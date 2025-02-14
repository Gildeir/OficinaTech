using Moq;
using OficinaTech.Application.Services;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Interfaces;

namespace OficinaTech.Tests
{
    public class FornecedorTests
    {
        private readonly Mock<IViaCepService> _viaCepServiceMock;
        private readonly FornecedorService _fornecedorService;

        public FornecedorTests()
        {
            _viaCepServiceMock = new Mock<IViaCepService>();
            _fornecedorService = new FornecedorService(_viaCepServiceMock.Object);
        }

        [Fact(DisplayName = "Buscar Endereço Deve Retornar Dados Corretos Quando CEP Existe")]
        public async Task BuscarEnderecoPorCepAsync_DeveRetornarEndereco_QuandoCepExiste()
        {
            // Arrange
            var enderecoEsperado = new Fornecedor { Cep = "01001000", Logradouro = "Praça da Sé", Bairro = "Sé", Cidade = "São Paulo", Estado = "SP" };
            _viaCepServiceMock.Setup(service => service.BuscarEnderecoPorCepAsync("01001000")).ReturnsAsync(enderecoEsperado);

            // Act
            var result = await _fornecedorService.BuscarEnderecoPorCepAsync("01001000");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Praça da Sé", result.Logradouro);
            Assert.Equal("Sé", result.Bairro);
            Assert.Equal("São Paulo", result.Cidade);
            Assert.Equal("SP", result.Estado);
        }
    }
}
