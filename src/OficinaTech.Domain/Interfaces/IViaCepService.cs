using OficinaTech.Domain.Entities;

namespace OficinaTech.Domain.Interfaces
{
    public interface IViaCepService
    {
        Task<Fornecedor> BuscarEnderecoPorCepAsync(string cep);

    }
}
