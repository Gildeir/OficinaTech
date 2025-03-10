using OficinaTech.Application.Common;
using OficinaTech.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaTech.Application.Interfaces
{
    public interface IPecaService
    {
        Task<Result<bool>> AddPecaAsync(Peca peca);
        Task<Result<List<Peca>>> GetAllPecasAsync();
        Task<Result<Peca?>> GetPecaByIdAsync(int id);
        Task<Result<bool>> UpdatePecaAsync(Peca peca);
        Task<Result<bool>> DeletePecaAsync(int id);
        Task<Result<bool>> ComprarPecaAsync(int id, int quantidade, decimal precoCusto);
    }
}
