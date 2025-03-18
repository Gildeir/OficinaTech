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

        /// <summary>
        /// Get all pecas
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<List<Peca>>> GetAllPecas()
        {
            var result = await _pecaService.GetAllPecasAsync();

            return Ok(result);
        }

        /// <summary>
        /// Get peca by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<Peca>> GetPecaById(int id)
        {
            var result = await _pecaService.GetPecaByIdAsync(id);
            
            if (!result.IsSuccess) return NotFound(new {sucess = false, data = result, erro = result.Error});
            
            return Ok(result);
        }


        /// <summary>
        /// Add peca
        /// </summary>
        /// <param name="peca"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult> AddPeca([FromBody] Peca peca)
        {
            var pecaResult = await _pecaService.AddPecaAsync(peca);

            if (!pecaResult.IsSuccess) return BadRequest("Erro ao adicionar a peça.");

            return CreatedAtAction(nameof(GetAllPecas), new { id = peca.Id }, peca);
        }

        /// <summary>
        /// Update peca
        /// </summary>
        /// <param name="id"></param>
        /// <param name="peca"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        public async Task<ActionResult>UpdatePeca(int id, [FromBody] Peca peca)
        {
            if (id != peca.Id) return BadRequest("Erro ao informar id");

            var pecaResult = await _pecaService.UpdatePecaAsync(peca);

            if (!pecaResult.IsSuccess)
                return NotFound(new { Success = false, data = pecaResult.Value, error = pecaResult.Error });

            return Ok($"{peca.Nome} atualizada com sucesso");
        }

        /// <summary>
        /// Delete peca
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id}")]
        public async Task<ActionResult>DeletePeca(int id)
        {
           var result = await _pecaService.DeletePecaAsync(id);

            if (!result.IsSuccess) return NotFound("Peça não encontrada para exclusão.");
            
            return Ok("Peça deletada");
        }

        /// <summary>
        /// Comprar peça para repor estoque.
        /// </summary>
        /// <param name="id">Identificador da peça</param>
        /// <param name="dto">Objeto contendo a quantidade e o preço de custo da peça</param>
        /// <returns>Retorna um IActionResult indicando o sucesso ou falha da compra</returns>


        [HttpPost("{id}/comprar-repor-estoque")]
        public async Task<IActionResult> ComprarPeca(int id, [FromBody] ComprarPecaDto dto)
        {
            if (dto.Quantidade <= 0 || dto.PrecoCusto <= 0)
                return BadRequest("Dados da compra inválidos.");
            
            var result = await _pecaService.ComprarPecaAsync(id, dto.Quantidade, dto.PrecoCusto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok("Compra realizada com sucesso");
        }

        /// <summary>
        /// Testar middleware de erro global
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        [HttpGet("testar-erro")]
        public IActionResult TestarErro()
        {
            throw new Exception("Erro simulado para testar o middleware!");
        }
    }
}
