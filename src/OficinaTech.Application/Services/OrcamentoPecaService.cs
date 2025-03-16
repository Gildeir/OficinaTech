using OficinaTech.Application.Common;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OficinaTech.Application.Services
{
    public class OrcamentoPecaService(
        IUnitOfWork _unitOfWork,
        IOrcamentoPecaRepository _orcamentoPecaRepository,
        IPecaRepository _pecaRepository,
        IOrcamentoRepository _orcamentoRepository,
        IServiceOrderRepository _serviceOrderRepository,
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

        public async Task<Result<bool>> UpdatePrecoEmOrcamentos(int pecaId, decimal novoPreco)
        {
            var orcamentoPecas = await _orcamentoPecaRepository.GetByPecaIdAsync(pecaId);

            if (!orcamentoPecas.Any())
                return Result<bool>.Failure("Erro ao encontrar o orçamento peças");

            foreach (var item in orcamentoPecas)
            {
                item.Peca.Preco = novoPreco;
                var result = await _orcamentoPecaRepository.UpdateAsync(item);
                if (!result)
                    return Result<bool>.Failure("Falha ao atualizar preço da peça em orçamento peça");
                return Result<bool>.Success(true);
            }
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> ExecutarOrdemServicoAsync(int orcamentoId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
               
                var orcamento = await _orcamentoRepository.GetByIdAsync(orcamentoId);

                if (orcamento == null)
                    return Result<bool>.Failure("Orçamento não encontrado."); 
                
                foreach (var orcamentoPeca in orcamento.OrcamentoPecas)
                {
                    if (orcamentoPeca.Status == EEstadoPecaOrcamento.Entregue)
                        return Result<bool>.Failure("Orçamento já executado, conferir ordem de seriço.");

                    var pecaId = orcamentoPeca.Peca.Id;
                    
                    if (orcamentoPeca == null)
                        return Result<bool>.Failure("Peça não encontrada no orçamento.");

                    var peca = orcamentoPeca.Peca;
                    if (peca == null)
                        return Result<bool>.Failure("Peça não encontrada no estoque.");

                    int quantidadeEntregue = 0;

                    if (peca.Estoque > orcamentoPeca.Quantidade)
                    {
                        quantidadeEntregue = orcamentoPeca.Quantidade;
                        peca.Estoque -= orcamentoPeca.Quantidade;
                        orcamentoPeca.Status = EEstadoPecaOrcamento.Entregue;
                        orcamentoPeca.LiberadaParaCompra = false;
                        orcamento.CalculaTotal();
                    }
                    else if (peca.Estoque == orcamentoPeca.Quantidade)
                    {
                        quantidadeEntregue = peca.Estoque;
                        peca.Estoque = 0;
                        orcamentoPeca.Status = EEstadoPecaOrcamento.Entregue;
                        orcamentoPeca.LiberadaParaCompra = true;
                        orcamento.CalculaTotal();
                    }
                    else
                    {
                        return Result<bool>.Failure("Quantidade de peça insuficiente no estoque. Favor, comprar mais peças.");
                    }

                    var updatedPeca = await _pecaRepository.UpdateAsync(peca);

                    if (orcamentoPeca == null)
                        return Result<bool>.Failure("OrcamentoPeca está nulo antes da atualização.");

                    var updatedOrcamentoPeca = await _orcamentoPecaRepository.UpdateAsync(orcamentoPeca);

                    if (!updatedPeca)
                    {
                        await _unitOfWork.RollbackAsync();
                        return Result<bool>.Failure("Falha ao atualizar peça estoque ");
                    }
                
                    if (!updatedOrcamentoPeca)
                    {
                        await _unitOfWork.RollbackAsync();
                        return Result<bool>.Failure("Falha ao atualizar o orçamento peça.");
                    }

                    var ordemServico = new ServiceOrder()
                    {
                        CreatedAt = DateTime.UtcNow,
                        Number = Guid.NewGuid().ToString(),
                        Orcamento = orcamento,
                        StatusServiceOrder = EStatusServiceOrder.Finalizado
                    };

                    var ordemServicoRegistrada = await _serviceOrderRepository.AddServiceOrder(ordemServico);

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
                        await _unitOfWork.RollbackAsync();
                        return Result<bool>.Failure("Movimentação de estoque não registrada.");
                    }

                    await _unitOfWork.CommitAsync();
                    return Result<bool>.Success(true);
                }

                await _unitOfWork.CommitAsync();
                return Result<bool>.Success(true);

            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Erro inesperado: {ex.Message}");
            }
        }
    }
}
