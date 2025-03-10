using OficinaTech.Domain.Common;
using OficinaTech.Domain.Entities;
using System.Threading.Tasks;

namespace OficinaTech.Domain.Interfaces
{
    public interface IViaCepService
    {
        Task<DomainResult<Fornecedor>> BuscarEnderecoPorCepAsync(string cep);
    }
}
