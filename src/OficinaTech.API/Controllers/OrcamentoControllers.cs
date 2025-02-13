using Microsoft.AspNetCore.Mvc;
using OficinaTech.Application.DTOs;
using OficinaTech.Application.Interfaces;

namespace OficinaTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrcamentoController : ControllerBase
    {
        private readonly IOrcamentoService _orcamentoService;

        public OrcamentoController(IOrcamentoService orcamentoService)
        {
            _orcamentoService = orcamentoService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarOrcamento([FromBody] CriarOrcamentoDto dto)
        {
            var orcamento = await _orcamentoService.CriarOrcamentoAsync(dto.Numero, dto.Placa, dto.Cliente);
            return CreatedAtAction(nameof(ObterOrcamentoPorId), new { id = orcamento.Id }, orcamento);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            return Ok(await _orcamentoService.ObterTodosOrcamentosAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterOrcamentoPorId(int id)
        {
            var orcamento = await _orcamentoService.ObterOrcamentoPorIdAsync(id);
            if (orcamento == null) return NotFound();
            return Ok(orcamento);
        }

        [HttpPost("{orcamentoId}/adicionar-peca")]
        public async Task<IActionResult> AdicionarPeca(int orcamentoId, [FromBody] AdicionarPecaDto dto)
        {
            var sucesso = await _orcamentoService.AdicionarPecaAoOrcamentoAsync(orcamentoId, dto.PecaId, dto.Quantidade);
            if (!sucesso) return BadRequest("Orçamento ou peça não encontrados.");
            return NoContent();
        }
    }

}
