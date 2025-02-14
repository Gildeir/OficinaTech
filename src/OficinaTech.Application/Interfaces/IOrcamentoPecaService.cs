using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Application.Interfaces
{
    public interface IOrcamentoPecaService
    {
        Task<bool> AddPecaToOrcamentoAsync(int orcamentoId, int pecaId, int quantidade);
        Task UpdatePrecoEmOrcamentos(int pecaId, decimal novoPreco);
    }
}
