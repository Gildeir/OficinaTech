using Microsoft.AspNetCore.Mvc;
using OficinaTech.Application.DTOs.OficinaTech.Application.DTOs;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;

namespace OficinaTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PecaController : ControllerBase
    {
        private readonly IPecaService _pecaService;
        public PecaController(IPecaService pecaService)
        {
            _pecaService = pecaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Peca>>> GetAllPecas()
        {
            var result = await _pecaService.GetAllPecasAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Peca>> GetPecaById(int id)
        {
            var result = await _pecaService.GetPecaByIdAsync(id);
            
            if (result == null) return NotFound();
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddPeca([FromBody] Peca peca)
        {
            var pecaResult = await _pecaService.AddPecaAsync(peca);

            if (!pecaResult) return BadRequest("Erro ao adicionar a peça.");

            return CreatedAtAction(nameof(GetAllPecas), new { id = peca.Id }, peca);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult>UpdatePeca(int id, [FromBody] Peca peca)
        {
            if (id != peca.Id) return BadRequest("Erro ao informar id");

            if (await _pecaService.UpdatePecaAsync(peca)) return NoContent();

            return NotFound("Peça não encontrada para atualização.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>DeletePeca(int id)
        {
           var result = await _pecaService.DeletePecaAsync(id);

            if (!result) return NotFound("Peça não encontrada para exclusão.");
            
            return NoContent();
        }

        [HttpPost("{id}/comprar-repor-estoque")]
        public async Task<IActionResult> ComprarPeca(int id, [FromBody] ComprarPecaDto dto)
        {
            if (dto == null || dto.Quantidade <= 0 || dto.PrecoCusto <= 0)
                return BadRequest("Dados da compra inválidos.");

           var statusPeca = _pecaService.GetPecaByIdAsync(id).Result;

           var success = await _pecaService.ComprarPecaAsync(id, dto.Quantidade, dto.PrecoCusto);

            if (!success)
                return NotFound("Peça não encontrada.");

            return NoContent(); // TODO Melhoras os retornos NoContent()
        }

    }
}
