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
        Task<bool> AddAsync(Orcamento orcamento);
        Task<List<Orcamento>> GetAllAsync();
        Task<Orcamento> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Orcamento orcamento);
        Task<bool> DeleteOrcamentoAsync(int id);
    }

}
