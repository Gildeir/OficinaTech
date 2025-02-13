using Microsoft.AspNetCore.Mvc;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.ExternalServices;

namespace OficinaTech.API.Controllers
{
    [ApiController]
    [Route("api/busca-fornecedores")]
    public class FornecedorController : ControllerBase
    {
        private readonly ViaCepService _viaCepService;

        public FornecedorController(ViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        [HttpGet("buscar-endereco/{cep}")]
        public async Task<IActionResult> ConsultaFornecedor(string cep)
        {
            var endereco = await _viaCepService.BuscarEnderecoPorCepAsync(cep);
            if (endereco == null)
                return BadRequest("CEP inválido ou não encontrado.");

             return Ok(endereco);
        }
    }

}
