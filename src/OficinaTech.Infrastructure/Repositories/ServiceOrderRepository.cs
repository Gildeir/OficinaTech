using Microsoft.EntityFrameworkCore;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Data.Context;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.Repositories
{
    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly OficinaTechDbContext _context;
        public ServiceOrderRepository(OficinaTechDbContext context)
        {
            _context = context;
        }
        public async Task<List<ServiceOrder>> GetAllServiceOrder()
        {
            var result = await _context.ServiceOrders
                .AsNoTracking()
                .Include(o => o.Orcamento)
                .ToListAsync();

            return result;
        }
        public async Task<ServiceOrder> GetServiceOrderById(int id)
        {
            var result = await _context.ServiceOrders
                .AsNoTracking()
                .Include(o => o.Orcamento)
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
        public async Task<bool> AddServiceOrder(ServiceOrder serviceOrder)
        {
           await _context.ServiceOrders.AddAsync(serviceOrder);

           return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteServiceOrder(int id)
        {
            var serviceOrder = await _context.ServiceOrders.FirstOrDefaultAsync(x => x.Id == id);

            _context.ServiceOrders.Remove(serviceOrder);

            return await _context.SaveChangesAsync() > 0;
        }


    }
}
