﻿using OficinaTech.Application.Common;
using OficinaTech.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaTech.Application.Interfaces
{
    public interface IOrcamentoService
    {
        Task<Result<Orcamento>> CreateOrcamentoAsync(string numero, string placa, string cliente);
        Task<Result<List<Orcamento>>> GetAllOrcamentosAsync();
        Task<Result<Orcamento>> GetOrcamentoByIdAsync(int id);
        Task<Result<bool>> DeleteOrcamentoAsync(int id);
    }

}
