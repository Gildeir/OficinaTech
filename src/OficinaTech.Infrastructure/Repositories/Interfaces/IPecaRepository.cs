using OficinaTech.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IPecaRepository
    {
        Task<List<Peca>> GetAllAsync();
        Task<Peca?> GetByIdAsync(int id);
        Task<bool> AddAsync(Peca peca);
        Task<bool> UpdateAsync(Peca peca);
        Task<bool> DeleteAsync(int id);
    }
}
