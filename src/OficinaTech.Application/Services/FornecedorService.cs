using OficinaTech.Application.Common;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Common;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Interfaces;
using OficinaTech.Infrastructure.ExternalServices;

namespace OficinaTech.Application.Services
{
    public class FornecedorService : IViaCepService
    {
        private readonly IViaCepService _viaCepService;

        public FornecedorService(IViaCepService viaCepService)
        {
            _viaCepService = viaCepService;
        }

        public async Task<DomainResult<Fornecedor>> BuscarEnderecoPorCepAsync(string cep)
        {
            var result = await _viaCepService.BuscarEnderecoPorCepAsync(cep);

            if (!result.IsSuccess || result.Value is null) return DomainResult<Fornecedor>.Failure("Não foi possível buscar o cep informado");

            return DomainResult<Fornecedor>.Success(result.Value);
        }
    }
}
