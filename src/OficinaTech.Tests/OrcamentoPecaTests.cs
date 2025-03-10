using Moq;
using OficinaTech.Application.Services;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OficinaTech.Tests
{
    public class OrcamentoPecaTests
    {
        private readonly Mock<IOrcamentoPecaRepository> _orcamentoPecaRepositoryMock;
        private readonly Mock<IPecaRepository> _pecaRepositoryMock;
        private readonly Mock<IMovimentacaoEstoqueRepository> _movimentacaoEstoqueRepositoryMock;
        private readonly OrcamentoPecaService _orcamentoPecaService;
        public OrcamentoPecaTests()
        {
            {
                _orcamentoPecaRepositoryMock = new Mock<IOrcamentoPecaRepository>();
                _pecaRepositoryMock = new Mock<IPecaRepository>();
                _movimentacaoEstoqueRepositoryMock = new Mock<IMovimentacaoEstoqueRepository>();
                //_orcamentoPecaService = new OrcamentoPecaService(
                //    _orcamentoPecaRepositoryMock.Object,
                //    _pecaRepositoryMock.Object,
                //    _movimentacaoEstoqueRepositoryMock.Object
                //);
            }
        }

        [Fact(DisplayName = "Adicionar Peça ao Orçamento Deve Retornar Verdadeiro Quando Bem Sucedido")]
        public async Task AddPecaToOrcamentoAsync_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var peca = new Peca { Id = 1, Nome = "Filtro de Óleo", Preco = 50, Estoque = 10 };
            _pecaRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(peca);
            _orcamentoPecaRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<OrcamentoPeca>())).ReturnsAsync(true);

            // Act
            var result = await _orcamentoPecaService.AddPecaToOrcamentoAsync(1, 1, 5);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact(DisplayName = "Atualizar Preço das Peças no Orçamento Deve Alterar Todos os Orçamentos com a Peça")]
        public async Task UpdatePrecoEmOrcamentos_DeveAtualizarPreco_QuandoPrecoMudou()
        {
            // Arrange
            var orcamentoPecas = new List<OrcamentoPeca>
            {
                new OrcamentoPeca { OrcamentoId = 1, Peca = new Peca { Id = 1, Preco = 50 } },
                new OrcamentoPeca { OrcamentoId = 2, Peca = new Peca { Id = 1, Preco = 50 } }
            };

            _orcamentoPecaRepositoryMock.Setup(repo => repo.GetByPecaIdAsync(1)).ReturnsAsync(orcamentoPecas);
            _orcamentoPecaRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<OrcamentoPeca>())).ReturnsAsync(true);

            // Act
            await _orcamentoPecaService.UpdatePrecoEmOrcamentos(1, 60);

            // Assert
            foreach (var orcamentoPeca in orcamentoPecas)
            {
                Assert.Equal(60, orcamentoPeca.Peca.Preco);
            }
        }
    }
}

