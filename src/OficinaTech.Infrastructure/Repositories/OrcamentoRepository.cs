using Microsoft.EntityFrameworkCore;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Data.Context;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Infrastructure.Repositories
{
    public class OrcamentoRepository : IOrcamentoRepository
    {
        private readonly OficinaTechDbContext _context;

        public OrcamentoRepository(OficinaTechDbContext context)
        {
            _context = context;
        }

        public async Task<List<Orcamento>> GetAllAsync()
        {
            var result = await _context.Orcamentos
                .Include(o => o.OrcamentoPecas)
                .ThenInclude(op => op.Peca).AsNoTracking().ToListAsync();

            return result;

        }
        public async Task<Orcamento?> GetByIdAsync(int id)
        {
            var result = await _context.Orcamentos
                .Include(o => o.OrcamentoPecas)
                .ThenInclude(op => op.Peca).AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id) ?? new();

            return result;
        }
        public async Task<bool> AddAsync(Orcamento orcamento)
        {
            await _context.Orcamentos.AddAsync(orcamento);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Orcamento orcamento)
        {
            _context.Orcamentos.Update(orcamento);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrcamentoAsync(int id)
        {
            var result = await _context.Orcamentos.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) return false;

            _context.Orcamentos.Remove(result);

            return await _context.SaveChangesAsync() > 0;


        }
    }

}
