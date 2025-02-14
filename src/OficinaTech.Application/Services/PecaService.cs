using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class PecaService : IPecaService
    {
        private readonly IPecaRepository _pecaRepository;
        private readonly IOrcamentoPecaService _orcamentopecaService;
        public PecaService(IPecaRepository pecaRepository, IOrcamentoPecaService orcamentoPecaService)
        {
            _pecaRepository = pecaRepository;
            _orcamentopecaService = orcamentoPecaService;
        }
        public async Task<List<Peca>> GetAllPecasAsync()
        {
            return await _pecaRepository.GetAllAsync();
        }

        public async Task<Peca> GetPecaByIdAsync(int id)
        {
            return await _pecaRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddPecaAsync(Peca peca)
        {
            return await _pecaRepository.AddAsync(peca);
        }

        public async Task<bool> UpdatePecaAsync(Peca peca)
        {
            return await _pecaRepository.UpdateAsync(peca);
        }
        public async Task<bool> DeletePecaAsync(int id)
        {
            return await _pecaRepository.DeleteAsync(id);
        }

        public async Task<bool> ComprarPecaAsync(int pecaId, int quantidadeReposicao, decimal precoCusto)
        {
            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null) return false; 

            peca.Estoque += quantidadeReposicao;

            
            if (peca.Preco < precoCusto)
            {
                peca.Preco = precoCusto;

                await _orcamentopecaService.UpdatePrecoEmOrcamentos(pecaId, precoCusto);
            }

            return await _pecaRepository.UpdateAsync(peca);
        }

    }
}
