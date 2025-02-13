using Microsoft.EntityFrameworkCore;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Data.Context;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Infrastructure.Repositories
{
    public class PecaRepository : IPecaRepository
    {
        private readonly OficinaTechDbContext _context;

        public PecaRepository(OficinaTechDbContext context)
        {
            _context = context;
        }

        public async Task<List<Peca>> GetAllAsync()
        {
            return await _context.Pecas.ToListAsync();
        }
        public async Task<bool> AddAsync(Peca peca)
        {
            await _context.Pecas.AddAsync(peca);
            
            return await _context.SaveChangesAsync() > 0;
        }


        public Task<Peca> GetByIdAsync(int id)
        {
            var result = _context.Pecas.FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<bool> UpdateAsync(Peca peca)
        {
            _context.Pecas.Update(peca);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
           var result = await _context.Pecas.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) return false;
                        
            _context.Pecas.Remove(result);
             
            return await _context.SaveChangesAsync() > 0;
            

        }

    }
}
