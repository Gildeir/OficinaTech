using OficinaTech.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        Task<bool> RegistrarMovimentacaoAsync(MovimentacaoEstoque movimentacao);
        Task<List<MovimentacaoEstoque>> GetMovimentacoesPorPecaAsync(int pecaId);
    }
}
