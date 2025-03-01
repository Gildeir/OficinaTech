using OficinaTech.Domain.Common;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Domain.Interfaces
{
    public interface IViaCepService
    {
        Task<DomainResult<Fornecedor>> BuscarEnderecoPorCepAsync(string cep);
    }
}
