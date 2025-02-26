using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Data.Context;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Infrastructure.Repositories
{
    public class OrcamentoPecaRepository : IOrcamentoPecaRepository
    {
        private readonly OficinaTechDbContext _context;
        public OrcamentoPecaRepository(OficinaTechDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(OrcamentoPeca orcamentoPeca)
        {
           var result = await _context.OrcamentoPecas.AddAsync(orcamentoPeca);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<OrcamentoPeca?> GetByOrcamentoAndPecaAsync(int orcamentoId, int pecaId)
        {
            var result = await _context.OrcamentoPecas
                .FirstOrDefaultAsync(x => x.OrcamentoId == orcamentoId && x.PecaId == pecaId);

            return result;
        }

        public async Task<List<OrcamentoPeca>> GetByPecaIdAsync(int pecaId)
        {
            return await _context.OrcamentoPecas
                .Where(op => op.PecaId == pecaId)
                .Include(op => op.Peca)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(OrcamentoPeca orcamentoPeca)
        {
            _context.OrcamentoPecas.Update(orcamentoPeca);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<bool> CommitTransactionAsync(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            try
            {
                await transaction.RollbackAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
