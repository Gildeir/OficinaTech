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

        public async Task<List<Orcamento>> ObterTodosAsync()
        {
            return await _context.Orcamentos.Include(o => o.OrcamentoPecas).ToListAsync();
        }
        public async Task<Orcamento> ObterPorIdAsync(int id)
        {
            return await _context.Orcamentos
                .Include(o => o.OrcamentoPecas)
                .FirstOrDefaultAsync(o => o.Id == id) ?? new Orcamento();
        }
        public async Task AdicionarAsync(Orcamento orcamento)
        {
            await _context.Orcamentos.AddAsync(orcamento);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Orcamento orcamento)
        {
            _context.Orcamentos.Update(orcamento);
            await _context.SaveChangesAsync();
        }
    }

}
