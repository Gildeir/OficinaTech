using OficinaTech.Domain.Entities;

namespace OficinaTech.Application.Interfaces
{
    public interface IPecaService
    {
        Task<bool> AddPecaAsync(Peca peca);
        Task<List<Peca>> GetAllPecasAsync();
        Task<Peca> GetPecaByIdAsync(int id);
        Task<bool> UpdatePecaAsync(Peca peca);
        Task<bool> DeletePecaAsync(int id);

    }
}
