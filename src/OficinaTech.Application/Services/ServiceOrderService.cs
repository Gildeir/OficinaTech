using OficinaTech.Application.Common;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Application.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        public ServiceOrderService(IServiceOrderRepository serviceOrderRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
        }
        public async Task<Result<List<ServiceOrder>>> GetAllServiceOrder()
        {
            var result = await _serviceOrderRepository.GetAllServiceOrder();

            if (result == null)
                return Result<List<ServiceOrder>>.Failure("Ordens de serviço não encontradas");

            return Result<List<ServiceOrder>>.Success(result);
        }
        public Task<Result<ServiceOrder>> GetServiceOrderById(int id)
        {
            throw new NotImplementedException();
        }
        private readonly IServiceOrderRepository _serviceOrderRepository;
        public Task<Result<bool>> AddServiceOrder(ServiceOrder serviceOrder)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteServiceOrder(int id)
        {
            throw new NotImplementedException();
        }


    }
}
