using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task<bool> CommitAsync();
        Task RollbackAsync();
    }
}