using Microsoft.AspNetCore.Mvc;
using OficinaTech.Application.Common;
using OficinaTech.Application.Interfaces;
using System.Threading.Tasks;

namespace OficinaTech.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceOrderController: Controller
    {
        private readonly IServiceOrderService _serviceOrderService;
        public ServiceOrderController(IServiceOrderService serviceOrderService)
        {
            _serviceOrderService = serviceOrderService;
        }

        /// <summary>
        /// Get all service order
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetAllServiceOrder()
        {
            var result = await _serviceOrderService.GetAllServiceOrder();

            //TODO (Gildeir): Add result pattern
            //if (!result.IsSuccess)
            //    return NotFound(new { success = false, errors = result.Error });

            return Ok(result);
        }
    }
}
