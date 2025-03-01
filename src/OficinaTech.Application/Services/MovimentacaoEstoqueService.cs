using System.Runtime.ExceptionServices;
using OficinaTech.Application.Common;
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

        public async Task<Result<bool>> RegistrarMovimentacaoAsync(int pecaId, int quantidade, ETipoMovimentacao tipo)
        {
            var movimentacao = new MovimentacaoEstoque
            {
                PecaId = pecaId,
                Quantidade = quantidade,
                Tipo = tipo
            };

            var result = await _movimentacaoEstoqueRepository.RegistrarMovimentacaoAsync(movimentacao);
            if (!result)
                return Result<bool>.Failure("Não foi possível registrar movimentação");

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<MovimentacaoEstoqueDto>>> GetMovimentacoesPorPecaAsync(int pecaId)
        {
            var movimentacoes = await _movimentacaoEstoqueRepository.GetMovimentacoesPorPecaAsync(pecaId);

            if (!movimentacoes.Any())
                return Result<List<MovimentacaoEstoqueDto>>.Failure("Falha ao obter movimentações");

            var result = movimentacoes.Select(m => new MovimentacaoEstoqueDto
            {
                Id = m.Id,
                PecaId = m.PecaId,
                NomePeca = m.Peca?.Nome,
                Quantidade = m.Quantidade,
                Tipo = m.Tipo == ETipoMovimentacao.Entrada ? "Entrada" : "Saída",
                DataMovimentacao = m.DataMovimentacao
            }).ToList();

            if (!result.Any())
                return Result<List<MovimentacaoEstoqueDto>>.Failure("Falha ao definir movimentações");

            return Result<List<MovimentacaoEstoqueDto>>.Success(result);
        }


    }
}
