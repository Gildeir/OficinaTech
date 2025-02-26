using OficinaTech.Application.Common;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class OrcamentoPecaService(
        IOrcamentoPecaRepository _orcamentoPecaRepository,
        IPecaRepository _pecaRepository,
        IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository
    ) : IOrcamentoPecaService
    {
        public async Task<Result<bool>> AddPecaToOrcamentoAsync(int orcamentoId, int pecaId, int quantidadeSolicitada)
        {
            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null)
                return Result<bool>.Failure("Peça não encontrada no sistema.");

            var orcamentoPecaExistente = await _orcamentoPecaRepository.GetByOrcamentoAndPecaAsync(orcamentoId, pecaId);

            if (orcamentoPecaExistente != null)
            {
                orcamentoPecaExistente.Quantidade += quantidadeSolicitada;

                orcamentoPecaExistente.LiberadaParaCompra = orcamentoPecaExistente.Quantidade >= peca.Estoque;
                orcamentoPecaExistente.Status = orcamentoPecaExistente.LiberadaParaCompra
                    ? EEstadoPecaOrcamento.LiberadaParaCompra
                    : EEstadoPecaOrcamento.EmEspera;

                var atualizado = await _orcamentoPecaRepository.UpdateAsync(orcamentoPecaExistente);
                if (!atualizado)
                    return Result<bool>.Failure("Falha ao atualizar o orçamento.");

                return Result<bool>.Success(true);
            }

            bool liberadaParaCompra = quantidadeSolicitada > peca.Estoque;

            var orcamentoPeca = new OrcamentoPeca
            {
                OrcamentoId = orcamentoId,
                PecaId = pecaId,
                Quantidade = quantidadeSolicitada,
                LiberadaParaCompra = liberadaParaCompra,
                Status = liberadaParaCompra
                    ? EEstadoPecaOrcamento.LiberadaParaCompra
                    : EEstadoPecaOrcamento.EmEspera
            };

            var isAdded = await _orcamentoPecaRepository.AddAsync(orcamentoPeca);

            if (!isAdded)
                return Result<bool>.Failure("Falha ao adicionar peça ao orçamento.");

            return Result<bool>.Success(true);
        }

        public async Task UpdatePrecoEmOrcamentos(int pecaId, decimal novoPreco)
        {
            var orcamentoPecas = await _orcamentoPecaRepository.GetByPecaIdAsync(pecaId);

            if (!orcamentoPecas.Any())
                return;

            foreach (var item in orcamentoPecas)
            {
                item.Peca.Preco = novoPreco;
                await _orcamentoPecaRepository.UpdateAsync(item);
            }

        }

        public async Task<Result<bool>> UsarPecaNoOrcamento(int orcamentoId, int pecaId)
        {
            try
            {
                await using var transaction = await _orcamentoPecaRepository.BeginTransactionAsync();

                var orcamentoPeca = await _orcamentoPecaRepository.GetByOrcamentoAndPecaAsync(orcamentoId, pecaId);
                if (orcamentoPeca == null)
                    return Result<bool>.Failure("Peça não encontrada no orçamento.");

                var peca = await _pecaRepository.GetByIdAsync(pecaId);
                if (peca == null)
                    return Result<bool>.Failure("Peça não encontrada no estoque.");

                int quantidadeDisponivel = peca.Estoque;
                int quantidadeNecessaria = orcamentoPeca.Quantidade;
                int quantidadeEntregue = 0;

                if (quantidadeDisponivel >= quantidadeNecessaria)
                {
                    quantidadeEntregue = quantidadeNecessaria;
                    peca.Estoque -= quantidadeNecessaria;
                    orcamentoPeca.Status = EEstadoPecaOrcamento.Entregue;
                    orcamentoPeca.LiberadaParaCompra = false;
                }
                else
                {
                    quantidadeEntregue = quantidadeDisponivel;
                    peca.Estoque = 0;
                    orcamentoPeca.Quantidade -= quantidadeEntregue;
                    orcamentoPeca.Status = EEstadoPecaOrcamento.LiberadaParaCompra;
                    orcamentoPeca.LiberadaParaCompra = true;
                }

                var updatedPeca = await _pecaRepository.UpdateAsync(peca);
                var updatedOrcamentoPeca = await _orcamentoPecaRepository.UpdateAsync(orcamentoPeca);

                if (!updatedPeca)
                {
                    await transaction.RollbackAsync();
                    return Result<bool>.Failure("Falha ao atualizar peça estoque ");
                }
                
                if (!updatedOrcamentoPeca)
                {
                    await transaction.RollbackAsync();
                    return Result<bool>.Failure("Falha ao atualizar o orçamento peça.");
                }

                var movimentacao = new MovimentacaoEstoque
                {
                    PecaId = pecaId,
                    Quantidade = quantidadeEntregue,
                    Tipo = ETipoMovimentacao.Saida,
                    DataMovimentacao = DateTime.UtcNow
                };

                var movimentacaoRegistrada = await _movimentacaoEstoqueRepository.RegistrarMovimentacaoAsync(movimentacao);
                if (!movimentacaoRegistrada)
                {
                    await transaction.RollbackAsync();
                    return Result<bool>.Failure("Movimentação de estoque não registrada.");
                }

                await transaction.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Erro inesperado: {ex.Message}");
            }
        }
    }
}
