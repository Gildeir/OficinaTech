using OficinaTech.Application.DTOs;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;

namespace OficinaTech.Application.Interfaces
{
    public interface IMovimentacaoEstoqueService
    {
        Task<bool> RegistrarMovimentacaoAsync(int pecaId, int quantidade, ETipoMovimentacao tipo);
        Task<List<MovimentacaoEstoqueDto>> GetMovimentacoesPorPecaAsync(int pecaId);
    }

}
