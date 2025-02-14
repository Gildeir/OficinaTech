using OficinaTech.Domain.Entities;

namespace OficinaTech.Application.Interfaces
{
    public interface IOrcamentoService
    {
        Task<Orcamento> CreateOrcamentoAsync(string numero, string placa, string cliente);
        Task<List<Orcamento>> GetAllOrcamentosAsync();
        Task<Orcamento> GetOrcamentoByIdAsync(int id);
        //Task<bool> AddPecaToOrcamentoAsync(int orcamentoId, int pecaId, int quantidade);
    }

}
