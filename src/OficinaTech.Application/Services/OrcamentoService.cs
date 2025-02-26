using OficinaTech.Application.Common;
using OficinaTech.Application.Factories;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class OrcamentoService : IOrcamentoService
    {
        private readonly IOrcamentoRepository _orcamentoRepository;

        public OrcamentoService(IOrcamentoRepository orcamentoRepository, IPecaRepository pecaRepository)
        {
            _orcamentoRepository = orcamentoRepository;
        }

        public async Task<Result<List<Orcamento>>> GetAllOrcamentosAsync()
        {
            var result = await _orcamentoRepository.GetAllAsync();

            if (!result.Any())
                return Result<List<Orcamento>>.Failure("Orçamentos não encontrado");
            
            return Result<List<Orcamento>>.Success(result);
        }

        public async Task<Result<Orcamento?>> GetOrcamentoByIdAsync(int id)
        {
            var result = await _orcamentoRepository.GetByIdAsync(id);
            
            if (result == null)
                return Result<Orcamento?>.Failure($"Orçamento id {id} não encontrado");

            return Result<Orcamento?>.Success(result);
        }
        
        public async Task<Result<Orcamento>> CreateOrcamentoAsync(string numero, string placa, string cliente)
        {
            var orcamento = OrcamentoFactory.CriarOrcamento(numero, placa, cliente);

            var result = await _orcamentoRepository.AddAsync(orcamento);

            if (!result)
                return Result<Orcamento>.Failure("Falha ao adicionar orçamento");

            return Result<Orcamento>.Success(orcamento);
        }

        public async Task<Result<bool>> DeleteOrcamentoAsync(int id)
        {
            var result = await _orcamentoRepository.DeleteOrcamentoAsync(id);
            if (!result)
                return Result<bool>.Failure($"Não foi possível deletar orçamcento {id}");

            return Result<bool>.Success(true);
        }
    }

}
