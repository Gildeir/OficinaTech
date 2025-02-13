using OficinaTech.Domain.Entities;

namespace OficinaTech.Application.Interfaces
{
    public interface IOrcamentoService
    {
        Task<Orcamento> CriarOrcamentoAsync(string numero, string placa, string cliente);
        Task<List<Orcamento>> ObterTodosOrcamentosAsync();
        Task<Orcamento> ObterOrcamentoPorIdAsync(int id);
        Task<bool> AdicionarPecaAoOrcamentoAsync(int orcamentoId, int pecaId, int quantidade);
    }

}
