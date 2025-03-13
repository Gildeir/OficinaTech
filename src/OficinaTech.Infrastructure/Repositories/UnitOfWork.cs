using Microsoft.EntityFrameworkCore.Storage;
using OficinaTech.Infrastructure.Data.Context;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OficinaTechDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(OficinaTechDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task<bool> CommitAsync()
        {
            if (_transaction == null) return false;
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                return true;
            }
            catch
            {
                await _transaction.RollbackAsync();
                return false;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }
    }
}