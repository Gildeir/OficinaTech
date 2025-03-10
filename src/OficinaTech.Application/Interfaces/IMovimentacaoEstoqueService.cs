using OficinaTech.Application.Common;
using OficinaTech.Application.DTOs;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaTech.Application.Interfaces
{
    public interface IMovimentacaoEstoqueService
    {
        Task<Result<bool>> RegistrarMovimentacaoAsync(int pecaId, int quantidade, ETipoMovimentacao tipo);
        Task<Result<List<MovimentacaoEstoqueDto>>> GetMovimentacoesPorPecaAsync(int pecaId);
    }

}
