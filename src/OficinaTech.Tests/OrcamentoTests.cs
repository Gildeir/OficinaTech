using Moq;

using OficinaTech.Application.Services;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Tests
{
    public class OrcamentoTests
    {
        private readonly Mock<IOrcamentoRepository> _orcamentoRepositoryMock;
        private readonly Mock<IPecaRepository> _pecaRepositoryMock;
        private readonly OrcamentoService _orcamentoService;
        private readonly Mock<IOrcamentoPecaRepository> _orcamentoPecaRepositoryMock;
        private readonly OrcamentoPecaService _orcamentoPecaService;



        public OrcamentoTests()
        {
            _orcamentoRepositoryMock = new Mock<IOrcamentoRepository>();
            _pecaRepositoryMock = new Mock<IPecaRepository>();
            _orcamentoService = new OrcamentoService(_orcamentoRepositoryMock.Object, _pecaRepositoryMock.Object);
            _orcamentoPecaRepositoryMock = new Mock<IOrcamentoPecaRepository>();

        }

        [Fact(DisplayName = "Obter Todos os Orçamentos Deve Retornar Lista de Orçamentos")]
        public async Task GetAllOrcamentosAsync_DeveRetornarListaDeOrcamentos()
        {
            // Arrange
            var orcamentos = new List<Orcamento>
            {
                new Orcamento { Id = 1, Numero = "001", PlacaVeiculo = "ABC1234", Cliente = "João" },
                new Orcamento { Id = 2, Numero = "002", PlacaVeiculo = "XYZ5678", Cliente = "Maria" }
            };
            _orcamentoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orcamentos);

            // Act
            var result = await _orcamentoService.GetAllOrcamentosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact(DisplayName = "Obter Orçamento Pelo ID Deve Retornar Orçamento Quando Existente")]
        public async Task GetOrcamentoByIdAsync_DeveRetornarOrcamento_QuandoExistente()
        {
            // Arrange
            _orcamentoRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => new Orcamento { Id = id, Numero = "001", PlacaVeiculo = "ABC1234", Cliente = "João" });

            // Act
            var result = await _orcamentoService.GetOrcamentoByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("001", result.Numero);
            Assert.Equal("ABC1234", result.PlacaVeiculo);
            Assert.Equal("João", result.Cliente);
        }

        [Fact(DisplayName = "Obter Orçamento Pelo ID Deve Retornar Nulo Quando Não Existente")]
        public async Task GetOrcamentoByIdAsync_DeveRetornarNulo_QuandoNaoExistente()
        {
            // Arrange
            _orcamentoRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Orcamento?)null);

            // Act
            var result = await _orcamentoService.GetOrcamentoByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Criar Novo Orçamento Deve Retornar Orçamento Criado")]
        public async Task CreateOrcamentoAsync_DeveRetornarOrcamentoCriado()
        {
            // Arrange
            var novoOrcamento = new Orcamento { Id = 1, Numero = "001", PlacaVeiculo = "ABC1234", Cliente = "João" };
            _orcamentoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Orcamento>())).ReturnsAsync(true);

            // Act
            var result = await _orcamentoService.CreateOrcamentoAsync("001", "ABC1234", "João");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("001", result.Numero);
            Assert.Equal("ABC1234", result.PlacaVeiculo);
            Assert.Equal("João", result.Cliente);
        }
    }
}
