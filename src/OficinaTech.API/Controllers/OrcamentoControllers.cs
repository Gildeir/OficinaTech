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

        [HttpPost]
        public async Task<IActionResult> CriarOrcamento([FromBody] CriarOrcamentoDto dto)
        {
            var orcamento = await _orcamentoService.CreateOrcamentoAsync(dto.Numero, dto.Placa, dto.Cliente);

            return CreatedAtAction(nameof(GetOrcamentoById), new { id = orcamento.Id }, orcamento);
        }

        [HttpGet("{id}")]
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

            var success = await _orcamentoPecaService.AddPecaToOrcamentoAsync(orcamentoId, dto.PecaId, dto.Quantidade);

            if (!success)
                return NotFound("Orçamento ou peça não encontrados.");

            return NoContent();
        }

        [HttpPost("{orcamentoId}/entregar-peca/{pecaId}")]
        public async Task<IActionResult> EntregarPeca(int orcamentoId, int pecaId)
        {
            var success = await _orcamentoPecaService.EntregarPecaAsync(orcamentoId, pecaId);

            if (!success) return BadRequest("Não foi possível entregar a peça. Verifique se há estoque suficiente.");

            return NoContent();
        }


    }

}
