using Microsoft.EntityFrameworkCore;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Data.Context;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Infrastructure.Repositories
{
    public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
    {
        private readonly OficinaTechDbContext _context;

        public MovimentacaoEstoqueRepository(OficinaTechDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarMovimentacaoAsync(MovimentacaoEstoque movimentacao)
        {
            await _context.MovimentacoesEstoque.AddAsync(movimentacao);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<MovimentacaoEstoque>> GetMovimentacoesPorPecaAsync(int pecaId)
        {
            return await _context.MovimentacoesEstoque
                .Where(x => x.PecaId == pecaId)
                .Include(x => x.Peca)
                . OrderByDescending(m => m.DataMovimentacao).ToListAsync();
        }
    }

}
