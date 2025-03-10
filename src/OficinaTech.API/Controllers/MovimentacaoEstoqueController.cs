using Microsoft.AspNetCore.Mvc;
using OficinaTech.Application.Interfaces;
using System.Threading.Tasks;

namespace OficinaTech.API.Controllers
{
    [ApiController]
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly IMovimentacaoEstoqueService _movimentacaoEstoqueService;
        public MovimentacaoEstoqueController(IMovimentacaoEstoqueService movimentacaoEstoqueService)
        {
            _movimentacaoEstoqueService = movimentacaoEstoqueService;
        }

        [HttpGet("{id}/controle-movimentacoes-estoque")]
        public async Task<IActionResult> GerarRelatorioMovimentacoes(int id)
        {
            var movimentacoes = await _movimentacaoEstoqueService.GetMovimentacoesPorPecaAsync(id);

            if (!movimentacoes.IsSuccess)
                return NotFound("Nenhuma movimentação encontrada para esta peça.");

            return Ok(movimentacoes);
        }
    }
}
