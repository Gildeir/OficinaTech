using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class PecaService : IPecaService
    {
        private readonly IPecaRepository _pecaRepository;
        public PecaService(IPecaRepository pecaRepository)
        {
            _pecaRepository = pecaRepository;
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
    }
}
