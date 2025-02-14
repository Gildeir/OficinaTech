using OficinaTech.Application.DTOs;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class MovimentacaoEstoqueService : IMovimentacaoEstoqueService
    {
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;

        public MovimentacaoEstoqueService(IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository)
        {
            _movimentacaoEstoqueRepository = movimentacaoEstoqueRepository;
        }

        public async Task<bool> RegistrarMovimentacaoAsync(int pecaId, int quantidade, ETipoMovimentacao tipo)
        {
            var movimentacao = new MovimentacaoEstoque
            {
                PecaId = pecaId,
                Quantidade = quantidade,
                Tipo = tipo
            };

            return await _movimentacaoEstoqueRepository.RegistrarMovimentacaoAsync(movimentacao);
        }

        public async Task<List<MovimentacaoEstoqueDto>> GetMovimentacoesPorPecaAsync(int pecaId)
        {
            var result = await _movimentacaoEstoqueRepository.GetMovimentacoesPorPecaAsync(pecaId);

            return result.Select(m => new MovimentacaoEstoqueDto
            {
                Id = m.Id,
                PecaId = m.PecaId,
                NomePeca = m.Peca?.Nome,
                Quantidade = m.Quantidade,
                Tipo = m.Tipo == ETipoMovimentacao.Entrada ? "Entrada" : "Saída",
                DataMovimentacao = m.DataMovimentacao
            }).ToList();
        }


    }
}
