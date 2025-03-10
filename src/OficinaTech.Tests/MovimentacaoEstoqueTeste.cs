using Moq;
using OficinaTech.Application.Interfaces;
using OficinaTech.Application.Services;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OficinaTech.Tests
{
    public class MovimentacaoEstoqueTests
    {
        private readonly Mock<IMovimentacaoEstoqueRepository> _movimentacaoEstoqueRepositoryMock;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;

        public MovimentacaoEstoqueTests()
        {
            _movimentacaoEstoqueRepositoryMock = new Mock<IMovimentacaoEstoqueRepository>();
            _movimentacaoEstoqueService = new MovimentacaoEstoqueService(_movimentacaoEstoqueRepositoryMock.Object);
        }

        [Fact(DisplayName = "Registrar Movimentação de Estoque Deve Retornar Verdadeiro Quando Bem Sucedido")]
        public async Task RegistrarMovimentacaoAsync_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            _movimentacaoEstoqueRepositoryMock.Setup(repo => repo.RegistrarMovimentacaoAsync(It.IsAny<MovimentacaoEstoque>())).ReturnsAsync(true);

            // Act
            var result = await _movimentacaoEstoqueService.RegistrarMovimentacaoAsync(1, 5, ETipoMovimentacao.Entrada);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact(DisplayName = "Obter Movimentações de Estoque Deve Retornar Lista Quando Existente")]
        public async Task GetMovimentacoesPorPecaAsync_DeveRetornarLista_QuandoExistente()
        {
            // Arrange
            var movimentacoes = new List<MovimentacaoEstoque>
            {
                new MovimentacaoEstoque { Id = 1, PecaId = 1, Quantidade = 5, Tipo = ETipoMovimentacao.Entrada },
                new MovimentacaoEstoque { Id = 2, PecaId = 1, Quantidade = 3, Tipo = ETipoMovimentacao.Saida }
            };
            _movimentacaoEstoqueRepositoryMock.Setup(repo => repo.GetMovimentacoesPorPecaAsync(1)).ReturnsAsync(movimentacoes);

            // Act
            var result = await _movimentacaoEstoqueService.GetMovimentacoesPorPecaAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Value.Count);
        }
    }
}
