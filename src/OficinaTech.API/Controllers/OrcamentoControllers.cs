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
            var result = await _orcamentoService.GetAllOrcamentosAsync();
            if (!result.IsSuccess)
                return NotFound(new { success = false, errors = result.Error });

            return Ok(new { success = true, data = result.Value });
        }

        [HttpPost("adicionar-orçamento")]
        public async Task<IActionResult> CriarOrcamento([FromBody] CriarOrcamentoDto dto)
        {
            //TODO (Gildeir): should refactor and implement fluent validation
            if (String.IsNullOrWhiteSpace(dto.Numero))
                return BadRequest(new { sucess = false, error = "A numeração do orçamento não pode ser nula." });

            if (String.IsNullOrWhiteSpace(dto.Cliente))
                return BadRequest(new { success = false, error = "O nome do cliente não pode ser nulo." });

            if (String.IsNullOrWhiteSpace(dto.Placa))
                return BadRequest(new { sucess = false, error = "A placa do veículo não pode ser nula"});

            var orcamento = await _orcamentoService.CreateOrcamentoAsync(dto.Numero, dto.Placa, dto.Cliente);

            if (!orcamento.IsSuccess)
                return BadRequest(new { success = false, error = orcamento.Error });

            return CreatedAtAction(nameof(GetOrcamentoById), new { id = orcamento?.Value?.Id }, orcamento);
        }

        [HttpGet("{id}/buscar-orcamento")]
        public async Task<IActionResult> GetOrcamentoById(int id)
        {
            var orcamento = await _orcamentoService.GetOrcamentoByIdAsync(id);

            if (!orcamento.IsSuccess) return NotFound(new { success = false, error = orcamento.Error });

            return Ok( new { sucess = true, data = orcamento.Value });
        }


        [HttpPost("{orcamentoId}/adicionar-peca")]
        public async Task<IActionResult> AddPeca(int orcamentoId, [FromBody] AdicionarPecaDto? dto)
        {
            if (dto == null)
                return BadRequest(new {success = false, error = "Os dados da peça não podem ser nulos." });

            var result = await _orcamentoPecaService.AddPecaToOrcamentoAsync(orcamentoId, dto.PecaId, dto.Quantidade);

            if (!result.IsSuccess)
                return NotFound(new { success = false, error = result.Error });

            return Ok( new { success = true, data = result.Value });
        }

        [HttpPost("{id}/entregar-peca/{pecaId}")]
        public async Task<IActionResult> EntregarPeca(int orcamentoId, int pecaId)
        {
            var success = await _orcamentoPecaService.UsarPecaNoOrcamento(orcamentoId, pecaId);

            if (!success.IsSuccess) return BadRequest("Não foi possível entregar a peça. Verifique se há estoque suficiente.");

            return Ok(success);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePeca(int id)
        {
            var result = await _orcamentoService.DeleteOrcamentoAsync(id);

            if (!result.IsSuccess) return NotFound("Orçamento não encontrado para exclusão.");

            return Ok("Orçamento deletado");
        }

    }

}
