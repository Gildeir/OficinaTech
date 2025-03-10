using OficinaTech.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IOrcamentoPecaRepository
    {
        Task<bool> AddAsync(OrcamentoPeca orcamentoPeca);
        Task<OrcamentoPeca?> GetByOrcamentoAndPecaAsync(int orcamentoId, int pecaId);
        Task<bool> UpdateAsync(OrcamentoPeca orcamentoPeca);
        Task<List<OrcamentoPeca>> GetByPecaIdAsync(int pecaId);
    }
}
