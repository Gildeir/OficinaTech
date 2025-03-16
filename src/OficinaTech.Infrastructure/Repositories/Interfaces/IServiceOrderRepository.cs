using OficinaTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IServiceOrderRepository
    {
        Task<List<ServiceOrder>> GetAllServiceOrder();
        Task<ServiceOrder> GetServiceOrderById(int id);
        Task<bool> AddServiceOrder(ServiceOrder serviceOrder);
        Task<bool> DeleteServiceOrder(int id);
    }
}
