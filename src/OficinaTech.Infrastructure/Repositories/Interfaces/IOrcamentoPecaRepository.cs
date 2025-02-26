﻿using Microsoft.EntityFrameworkCore.Storage;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Infrastructure.Repositories.Interfaces
{
    public interface IOrcamentoPecaRepository
    {
        Task<bool> AddAsync(OrcamentoPeca orcamentoPeca);
        Task<OrcamentoPeca?> GetByOrcamentoAndPecaAsync(int orcamentoId, int pecaId);
        Task<bool> UpdateAsync(OrcamentoPeca orcamentoPeca);
        Task<List<OrcamentoPeca>> GetByPecaIdAsync(int pecaId);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<bool> CommitTransactionAsync(IDbContextTransaction transaction);
        Task<bool> RollbackTransactionAsync(IDbContextTransaction transaction);

    }
}
