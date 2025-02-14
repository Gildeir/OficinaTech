using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        Task<bool> RegistrarMovimentacaoAsync(MovimentacaoEstoque movimentacao);
        Task<List<MovimentacaoEstoque>> GetMovimentacoesPorPecaAsync(int pecaId);
    }
}
