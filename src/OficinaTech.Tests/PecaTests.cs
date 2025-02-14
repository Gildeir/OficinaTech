
using System.Net;
using System.Runtime.Intrinsics.X86;
using System;
using Moq;
using OficinaTech.Application.Interfaces;
using OficinaTech.Application.Services;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Tests
{
    // AAA(Arrange, Act, Assert) → Organiza o código dentro dos testes.
    // Given-When-Then → Nomeia os testes de forma descritiva.
    // Método_DeveFazerAlgo_QuandoAlgumaCondicao()


    public class PecaServiceTests
    {
        private readonly Mock<IPecaRepository> _pecaRepositoryMock;
        private readonly Mock<IMovimentacaoEstoqueRepository> _movimentacaoEstoqueRepositoryMock;
        private readonly Mock<IOrcamentoPecaService> _orcamentoPecaServiceMock;
        private readonly PecaService _pecaService;

        public PecaServiceTests()
        {
            _pecaRepositoryMock = new Mock<IPecaRepository>();

            _movimentacaoEstoqueRepositoryMock = new Mock<IMovimentacaoEstoqueRepository>();

            _orcamentoPecaServiceMock = new Mock<IOrcamentoPecaService>();

            _pecaService = new PecaService(_pecaRepositoryMock.Object, _orcamentoPecaServiceMock.Object, _movimentacaoEstoqueRepositoryMock.Object);
        }

        [Fact(DisplayName = "Get Todas as Peças Deve Retornar Lista de Peças")]
        public async Task GetAllPecasAsync_DeveRetornarListaDePecas()
        {
            // Arrange
            var pecas = new List<Peca>
            {
                new Peca { Id = 1, Nome = "Filtro de Óleo", Preco = 50, Estoque = 10 },
                new Peca { Id = 2, Nome = "Pastilha de Freio", Preco = 100, Estoque = 5 }
            };
            _pecaRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(pecas);

            // Act
            var result = await _pecaService.GetAllPecasAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact(DisplayName = "Obter Peça Pelo ID Deve Retornar Peça Quando Existente")]
        public async Task GetPecaByIdAsync_DeveRetornarPeca_QuandoExistente()
        {
            // Arrange
            _pecaRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => new Peca { Id = id, Nome = "Filtro de Óleo", Preco = 50, Estoque = 10 });

            // Act
            var result = await _pecaService.GetPecaByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Filtro de Óleo", result.Nome);
            Assert.Equal(50, result.Preco);
            Assert.Equal(10, result.Estoque);
        }

        [Fact(DisplayName = "Comprar Peça Deve Atualizar Estoque e Preço Quando Necessário")]
        public async Task ComprarPecaAsync_DeveAtualizarEstoqueEPreco_QuandoPrecoDiferente()
        {
            // Arrange
            var peca = new Peca { Id = 1, Nome = "Filtro de Óleo", Preco = 50, Estoque = 10 };

            _pecaRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(peca);
            _pecaRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Peca>())).ReturnsAsync(true);

            
            _movimentacaoEstoqueRepositoryMock
                .Setup(repo => repo.RegistrarMovimentacaoAsync(It.IsAny<MovimentacaoEstoque>()))
                .ReturnsAsync(true); // Garantindo que a movimentação foi registrada com sucesso

            // Act
            var success = await _pecaService.ComprarPecaAsync(1, 5, 50);

            // Assert
            Assert.True(success); // Agora deve passar!
            Assert.Equal(15, peca.Estoque); // O estoque deve ter aumentado corretamente
            Assert.Equal(50, peca.Preco); // O preço deve permanecer o mesmo
        }

        [Fact(DisplayName = "Adicionar Peça Deve Retornar Verdadeiro Quando Bem Sucedido")]
        public async Task AddPecaAsync_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var novaPeca = new Peca { Id = 3, Nome = "Correia Dentada", Preco = 200, Estoque = 20 };
            _pecaRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Peca>())).ReturnsAsync(true);

            // Act
            var result = await _pecaService.AddPecaAsync(novaPeca);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Atualizar Peça Deve Retornar Verdadeiro Quando Bem Sucedido")]
        public async Task UpdatePecaAsync_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var peca = new Peca { Id = 1, Nome = "Filtro de Óleo", Preco = 50, Estoque = 10 };
            _pecaRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Peca>())).ReturnsAsync(true);

            // Act
            var result = await _pecaService.UpdatePecaAsync(peca);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Excluir Peça Deve Retornar Verdadeiro Quando Bem Sucedido")]
        public async Task DeletePecaAsync_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            _pecaRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _pecaService.DeletePecaAsync(1);

            // Assert
            Assert.True(result);
        }

    }
}
