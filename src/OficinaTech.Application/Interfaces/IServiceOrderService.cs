using OficinaTech.Application.Common;
using OficinaTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Application.Interfaces
{
    public interface IServiceOrderService
    {
        Task<Result<List<ServiceOrder>>> GetAllServiceOrder();
        Task<Result<ServiceOrder>> GetServiceOrderById(int id);
        Task<Result<bool>> AddServiceOrder(ServiceOrder serviceOrder);
        Task<Result<bool>> DeleteServiceOrder(int id);
    }
}
