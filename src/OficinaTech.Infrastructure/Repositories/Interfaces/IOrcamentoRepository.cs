using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IOrcamentoRepository
    {
        Task AdicionarAsync(Orcamento orcamento);
        Task<List<Orcamento>> ObterTodosAsync();
        Task<Orcamento> ObterPorIdAsync(int id);
        Task AtualizarAsync(Orcamento orcamento);
    }

}
