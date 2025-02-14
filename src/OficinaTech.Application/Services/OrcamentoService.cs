using OficinaTech.Application.Factories;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class OrcamentoService : IOrcamentoService
    {
        private readonly IOrcamentoRepository _orcamentoRepository;

        public OrcamentoService(IOrcamentoRepository orcamentoRepository, IPecaRepository pecaRepository)
        {
            _orcamentoRepository = orcamentoRepository;
        }

        public async Task<List<Orcamento>> GetAllOrcamentosAsync()
        {
            return await _orcamentoRepository.GetAllAsync();
        }

        public async Task<Orcamento> GetOrcamentoByIdAsync(int id)
        {
            return await _orcamentoRepository.GetByIdAsync(id);
        }

        public async Task<Orcamento> CreateOrcamentoAsync(string numero, string placa, string cliente)
        {
            var orcamento = OrcamentoFactory.CriarOrcamento(numero, placa, cliente);

            await _orcamentoRepository.AddAsync(orcamento);

            return orcamento;
        }

        public async Task<bool> DeleteOrcamentoAsync(int id)
        {
            return await _orcamentoRepository.DeleteOrcamentoAsync(id);
        }
    }

}
