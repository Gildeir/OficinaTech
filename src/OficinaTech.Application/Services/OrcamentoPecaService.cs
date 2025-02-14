using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class OrcamentoPecaService : IOrcamentoPecaService
    {
        private readonly IOrcamentoPecaRepository _orcamentoPecaRepository;
        private readonly IPecaRepository _pecaRepository;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;

        public OrcamentoPecaService(IOrcamentoPecaRepository orcamentoPecaRepository, 
            IPecaRepository pecaRepository, IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository)
        {
            _orcamentoPecaRepository = orcamentoPecaRepository;
            _pecaRepository = pecaRepository;
            _movimentacaoEstoqueRepository = movimentacaoEstoqueRepository;
        }

        public async Task<bool> AddPecaToOrcamentoAsync(int orcamentoId, int pecaId, int quantidadeSolicitada)
        {
            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null) return false;

            var orcamentoPecaExistente = await _orcamentoPecaRepository.GetByOrcamentoAndPecaAsync(orcamentoId, pecaId);

            if (orcamentoPecaExistente != null)
            {
                
                orcamentoPecaExistente.Quantidade += quantidadeSolicitada;

                if (orcamentoPecaExistente.Quantidade >= peca.Estoque)
                {
                    orcamentoPecaExistente.LiberadaParaCompra = true;
                    orcamentoPecaExistente.Status = EEstadoPecaOrcamento.LiberadaParaCompra;
                }
                else
                {
                    orcamentoPecaExistente.LiberadaParaCompra = false;
                    orcamentoPecaExistente.Status = EEstadoPecaOrcamento.EmEspera;
                }


                return await _orcamentoPecaRepository.UpdateAsync(orcamentoPecaExistente);
            }

            bool liberadaParaCompra = quantidadeSolicitada > peca.Estoque;

            var orcamentoPeca = new OrcamentoPeca
            {
                OrcamentoId = orcamentoId,
                PecaId = pecaId,
                Quantidade = quantidadeSolicitada,
                LiberadaParaCompra = liberadaParaCompra,
                Status = liberadaParaCompra ? EEstadoPecaOrcamento.LiberadaParaCompra : EEstadoPecaOrcamento.EmEspera
            };

            return await _orcamentoPecaRepository.AddAsync(orcamentoPeca);
        }


        public async Task UpdatePrecoEmOrcamentos(int pecaId, decimal novoPreco)
        {
            var orcamentoPecas = await _orcamentoPecaRepository.GetByPecaIdAsync(pecaId);

            foreach (var item in orcamentoPecas)
            {
                item.Peca.Preco = novoPreco;
                await _orcamentoPecaRepository.UpdateAsync(item);
            }
        }

        public async Task<bool> UsarPecaNoOrcamento(int orcamentoId, int pecaId)
        {
            var orcamentoPeca = await _orcamentoPecaRepository.GetByOrcamentoAndPecaAsync(orcamentoId, pecaId);
            if (orcamentoPeca == null)
                throw new Exception("Peça não encontrada no orçamento.");

            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null)
                throw new Exception("Peça não encontrada no estoque.");

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

            await _pecaRepository.UpdateAsync(peca);
            await _orcamentoPecaRepository.UpdateAsync(orcamentoPeca);

            
            var movimentacao = new MovimentacaoEstoque
            {
                PecaId = pecaId,
                Quantidade = quantidadeEntregue,
                Tipo = ETipoMovimentacao.Saida,
                DataMovimentacao = DateTime.UtcNow
            };

            return await _movimentacaoEstoqueRepository.RegistrarMovimentacaoAsync(movimentacao);
        }



    }
}

