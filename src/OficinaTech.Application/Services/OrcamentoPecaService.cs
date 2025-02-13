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

        public OrcamentoPecaService(IOrcamentoPecaRepository orcamentoPecaRepository, IPecaRepository pecaRepository)
        {
            _orcamentoPecaRepository = orcamentoPecaRepository;
            _pecaRepository = pecaRepository;
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

        public async Task<bool> EntregarPecaAsync(int orcamentoId, int pecaId)
        {
            var orcamentoPeca = await _orcamentoPecaRepository.GetByOrcamentoAndPecaAsync(orcamentoId, pecaId);
            if (orcamentoPeca == null) return false; // A peça não foi encontrada no orçamento

            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null) return false; // A peça não existe no cadastro

            // Verifica se há estoque suficiente para entrega
            if (peca.Estoque < orcamentoPeca.Quantidade) return false;

            // Atualiza o estoque da peça
            peca.Estoque -= orcamentoPeca.Quantidade;

            // Atualiza o status da peça no orçamento para entregue
            orcamentoPeca.Status = EEstadoPecaOrcamento.Entregue;

            // Salva as alterações no banco
            await _pecaRepository.UpdateAsync(peca);
            return await _orcamentoPecaRepository.UpdateAsync(orcamentoPeca);
        }


    }
}

