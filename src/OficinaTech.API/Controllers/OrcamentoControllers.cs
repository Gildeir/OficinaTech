using Microsoft.AspNetCore.Mvc;
using OficinaTech.Application.DTOs;
using OficinaTech.Application.Interfaces;
using OficinaTech.Application.Services;

namespace OficinaTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrcamentoController : ControllerBase
    {
        private readonly IOrcamentoService _orcamentoService;
        private readonly IOrcamentoPecaService _orcamentoPecaService;


        public OrcamentoController(IOrcamentoService orcamentoService, IOrcamentoPecaService orcamentoPecaService)
        {
            _orcamentoService = orcamentoService;
            _orcamentoPecaService = orcamentoPecaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrcamentos()
        {
            return Ok(await _orcamentoService.GetAllOrcamentosAsync());
        }

        [HttpPost("adicionar-orçamento")]
        public async Task<IActionResult> CriarOrcamento([FromBody] CriarOrcamentoDto dto)
        {
  
            if (String.IsNullOrWhiteSpace(dto.Numero))
                return BadRequest("A numeração do orçamento não pode ser nula.");

            if (String.IsNullOrWhiteSpace(dto.Cliente))
                return BadRequest("O nome do cliente não pode ser nulo.");

            if (String.IsNullOrWhiteSpace(dto.Placa))
                return BadRequest("A placa não pode ser nula.");


            var orcamento = await _orcamentoService.CreateOrcamentoAsync(dto.Numero, dto.Placa, dto.Cliente);

            return CreatedAtAction(nameof(GetOrcamentoById), new { id = orcamento.Id }, orcamento);
        }

        [HttpGet("{id}/buscar-orcamento")]
        public async Task<IActionResult> GetOrcamentoById(int id)
        {
            var orcamento = await _orcamentoService.GetOrcamentoByIdAsync(id);

            if (orcamento == null) return NotFound();

            return Ok(orcamento);
        }


        [HttpPost("{orcamentoId}/adicionar-peca")]
        public async Task<IActionResult> AddPeca(int orcamentoId, [FromBody] AdicionarPecaDto dto)
        {
            if (dto == null)
                return BadRequest("Os dados da peça não podem ser nulos.");

            var isAdded = await _orcamentoPecaService.AddPecaToOrcamentoAsync(orcamentoId, dto.PecaId, dto.Quantidade);

            if (!isAdded)
                return NotFound("Orçamento ou peça não encontrados.");

            return Ok($"Sucesso ao adicionar a peça");
        }

        [HttpPost("{id}/entregar-peca/{pecaId}")]
        public async Task<IActionResult> EntregarPeca(int orcamentoId, int pecaId)
        {
            var success = await _orcamentoPecaService.UsarPecaNoOrcamento(orcamentoId, pecaId);

            if (!success) return BadRequest("Não foi possível entregar a peça. Verifique se há estoque suficiente.");

            return Ok(success);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePeca(int id)
        {
            var result = await _orcamentoService.DeleteOrcamentoAsync(id);

            if (!result) return NotFound("Orçamento não encontrado para exclusão.");

            return Ok("Orçamento deletado");
        }

    }

}
