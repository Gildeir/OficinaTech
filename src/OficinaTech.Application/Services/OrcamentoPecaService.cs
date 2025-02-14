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

        public OrcamentoPecaService(IOrcamentoPecaRepository orcamentoPecaRepository, IPecaRepository pecaRepository, IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository)
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
                // Evita duplicidade na criação de orçamento
                orcamentoPecaExistente.Quantidade += quantidadeSolicitada;
                return await _orcamentoPecaRepository.UpdateAsync(orcamentoPecaExistente);
            }

            bool liberadaParaCompra = quantidadeSolicitada > peca.Estoque;

            var orcamentoPeca = new OrcamentoPeca
            {
                OrcamentoId = orcamentoId,
                PecaId = pecaId,
                Quantidade = quantidadeSolicitada,
                LiberadaParaCompra = liberadaParaCompra,
                Status = EEstadoPecaOrcamento.EmEspera
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
            if (orcamentoPeca == null) return false;

            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null) return false;

            
            if (peca.Estoque < orcamentoPeca.Quantidade) return false;

            
            peca.Estoque -= orcamentoPeca.Quantidade;

            
            orcamentoPeca.Status = EEstadoPecaOrcamento.Entregue;

            
            await _pecaRepository.UpdateAsync(peca);

            await _orcamentoPecaRepository.UpdateAsync(orcamentoPeca);

            var movimentacao = new MovimentacaoEstoque
            {
                PecaId = pecaId,
                Quantidade = orcamentoPeca.Quantidade,
                Tipo = ETipoMovimentacao.Saida
            };

            return await _movimentacaoEstoqueRepository.RegistrarMovimentacaoAsync(movimentacao);
        }


    }
}

