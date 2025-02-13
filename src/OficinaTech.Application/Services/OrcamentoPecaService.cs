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

        public async Task<bool> AddPecaToOrcamentoAsync(int orcamentoId, int pecaId, int quantidade)
        {
            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null) return false;

            var orcamentoPecaExistente = await _orcamentoPecaRepository.GetByOrcamentoAndPecaAsync(orcamentoId, pecaId);
            if (orcamentoPecaExistente != null)
            {
                orcamentoPecaExistente.Quantidade += quantidade;
                return await _orcamentoPecaRepository.UpdateAsync(orcamentoPecaExistente);
            }

            bool liberadaParaCompra = quantidade > peca.Estoque;

            var orcamentoPeca = new OrcamentoPeca
            {
                OrcamentoId = orcamentoId,
                PecaId = pecaId,
                Quantidade = quantidade,
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

    }
}

