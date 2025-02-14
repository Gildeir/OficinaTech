using System.Text.Json;
using System.Text.Json.Serialization;
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

        public async Task<Fornecedor> BuscarEnderecoPorCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var endereco = JsonSerializer.Deserialize<ViaCepResponse>(json);

            return new Fornecedor
            {
                Cep = cep,
                Logradouro = endereco.Logradouro,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado
            };
        }
    }
    public class ViaCepResponse
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("uf")]
        public string Estado { get; set; }
    }
}
