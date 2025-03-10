using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OficinaTech.Domain.Common;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Interfaces;

namespace OficinaTech.Infrastructure.ExternalServices
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DomainResult<Fornecedor>> BuscarEnderecoPorCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (!response.IsSuccessStatusCode)
                return DomainResult<Fornecedor>.Failure("Erro ao acessar a API ViaCEP.");

            var json = await response.Content.ReadAsStringAsync();
            var endereco = JsonSerializer.Deserialize<ViaCepResponse>(json);

            if (endereco == null)
                return DomainResult<Fornecedor>.Failure("Resposta inválida da API ViaCEP.");

            return DomainResult<Fornecedor>.Success(new Fornecedor
            {
                Cep = cep,
                Logradouro = endereco.Logradouro ?? string.Empty,
                Bairro = endereco.Bairro ?? string.Empty,
                Cidade = endereco.Cidade ?? string.Empty,
                Estado = endereco.Estado ?? string.Empty
            });
        }
    }

    public class ViaCepResponse
    {
        [JsonPropertyName("cep")]
        public string? Cep { get; set; }

        [JsonPropertyName("logradouro")]
        public string? Logradouro { get; set; }

        [JsonPropertyName("bairro")]
        public string? Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string? Cidade { get; set; }

        [JsonPropertyName("uf")]
        public string? Estado { get; set; }
    }
}