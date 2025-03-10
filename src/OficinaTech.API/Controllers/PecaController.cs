using Microsoft.AspNetCore.Mvc;
using OficinaTech.Application.DTOs.OficinaTech.Application.DTOs;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PecaController : ControllerBase
    {
        private readonly IPecaService _pecaService;
        public PecaController(IPecaService pecaService, IMovimentacaoEstoqueService movimentacaoEstoqueService)
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
            
            if (!result.IsSuccess) return NotFound(new {sucess = false, data = result, erro = result.Error});
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddPeca([FromBody] Peca peca)
        {
            var pecaResult = await _pecaService.AddPecaAsync(peca);

            if (!pecaResult.IsSuccess) return BadRequest("Erro ao adicionar a peça.");

            return CreatedAtAction(nameof(GetAllPecas), new { id = peca.Id }, peca);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult>UpdatePeca(int id, [FromBody] Peca peca)
        {
            if (id != peca.Id) return BadRequest("Erro ao informar id");

            var pecaResult = await _pecaService.UpdatePecaAsync(peca);

            if (!pecaResult.IsSuccess)
                return NotFound(new { Success = false, data = pecaResult.Value, error = pecaResult.Error });

            return Ok($"{peca.Nome} atualizada com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>DeletePeca(int id)
        {
           var result = await _pecaService.DeletePecaAsync(id);

            if (!result.IsSuccess) return NotFound("Peça não encontrada para exclusão.");
            
            return Ok("Peça deletada");
        }

        [HttpPost("{id}/comprar-repor-estoque")]
        public async Task<IActionResult> ComprarPeca(int id, [FromBody] ComprarPecaDto dto)
        {
            if (dto.Quantidade <= 0 || dto.PrecoCusto <= 0)
                return BadRequest("Dados da compra inválidos.");
            
            var result = await _pecaService.ComprarPecaAsync(id, dto.Quantidade, dto.PrecoCusto);

            if (!result.IsSuccess)
                return NotFound("Peça não encontrada.");

            return Ok("Compra realizada com sucesso");
        }

        [HttpGet("testar-erro")]
        public IActionResult TestarErro()
        {
            throw new Exception("Erro simulado para testar o middleware!");
        }


    }
}
