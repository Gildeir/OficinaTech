using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Interfaces;

namespace OficinaTech.Application.Services
{
    public class FornecedorService : IViaCepService
    {
        private readonly IViaCepService _viaCepService;

        public FornecedorService(IViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        public Task<Fornecedor> BuscarEnderecoPorCepAsync(string cep)
        {
            return _viaCepService.BuscarEnderecoPorCepAsync(cep);
        }
    }
}
