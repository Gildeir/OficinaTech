using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OficinaTech.Application.Common;

namespace OficinaTech.Application.Interfaces
{
    public interface IOrcamentoPecaService
    {
        Task<Result<bool>> AddPecaToOrcamentoAsync(int orcamentoId, int pecaId, int quantidade);
        Task<Result<bool>> UpdatePrecoEmOrcamentos(int pecaId, decimal novoPreco);
        Task<Result<bool>> UsarPecaNoOrcamento(int orcamentoId, int pecaId);
    }
}
