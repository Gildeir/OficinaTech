using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IOrcamentoPecaRepository
    {
        Task<bool> AddAsync(OrcamentoPeca orcamentoPeca);
        Task<OrcamentoPeca> GetByOrcamentoAndPecaAsync(int orcamentoId, int pecaId);
        Task<bool> UpdateAsync(OrcamentoPeca orcamentoPeca);
    }
}
