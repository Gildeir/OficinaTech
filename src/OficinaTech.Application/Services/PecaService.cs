using OficinaTech.Application.Common;
using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OficinaTech.Application.Services
{
    public class PecaService : IPecaService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IPecaRepository _pecaRepository;
        private readonly IOrcamentoPecaService _orcamentoPecaService;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;
        private readonly IOrcamentoPecaRepository _orcamentoPecaRepository;

        public PecaService(IPecaRepository pecaRepository, IOrcamentoPecaService orcamentoPecaService, IUnitOfWork unitOfWork,
            IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository, IOrcamentoPecaRepository orcamentoPecaRepository)
        {
            _pecaRepository = pecaRepository;
            _orcamentoPecaService = orcamentoPecaService;
            _movimentacaoEstoqueRepository = movimentacaoEstoqueRepository;
            _orcamentoPecaRepository = orcamentoPecaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<Peca>>> GetAllPecasAsync()
        {
            var result = await _pecaRepository.GetAllAsync();

            if (result == null)
                return Result<List<Peca>>.Failure("Peças não encontradas");

            return Result<List<Peca>>.Success(result);
        }

        public async Task<Result<Peca>> GetPecaByIdAsync(int id)
        {
            var result = await _pecaRepository.GetByIdAsync(id);

            if (result == null)
                return Result<Peca>.Failure($"Peça id {id} não encontrada");

            return Result<Peca>.Success(result);

        }

        public async Task<Result<bool>> AddPecaAsync(Peca peca)
        {
            var result = await _pecaRepository.AddAsync(peca);

            if (!result)
                return Result<bool>.Failure("Erro ao adicionar peça");

            return Result<bool>.Success(result);
        }

        public async Task<Result<bool>> UpdatePecaAsync(Peca peca)
        {
            var result = await _pecaRepository.UpdateAsync(peca);
            if (!result)
                return Result<bool>.Failure("Erro ao atualizar peça");

            return Result<bool>.Success(result);
        }
        public async Task<Result<bool>> DeletePecaAsync(int id)
        {
            var result = await _pecaRepository.DeleteAsync(id);

            if (!result)
                return Result<bool>.Failure("Erro ao deletar peça");

            return Result<bool>.Success(result);
        }

        public async Task<Result<bool>> ComprarPecaAsync(int pecaId, int quantidadeReposicao, decimal precoCusto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var peca = await _pecaRepository.GetByIdAsync(pecaId);
                if (peca == null)
                    return Result<bool>.Failure($"Erro ao buscar peça id {pecaId}");

                var orcamentoPeca = await _orcamentoPecaRepository.GetByPecaIdAsync(pecaId);

                foreach (var item in orcamentoPeca)
                {
                    if (item.LiberadaParaCompra == false)
                    {
                        await _unitOfWork.RollbackAsync();
                        return Result<bool>.Failure("Peça não está liberada para compra.");
                    }
                }

                peca.Estoque += quantidadeReposicao;

                foreach (var item in orcamentoPeca)
                {
                    if(item.Quantidade <= item.Peca.Estoque)
                    {
                        item.LiberadaParaCompra = false;
                        item.Status = EEstadoPecaOrcamento.Comprada;
                    }
                }

                if (peca.Preco < precoCusto)
                {
                    peca.Preco = precoCusto;
                    await _orcamentoPecaService.UpdatePrecoEmOrcamentos(pecaId, precoCusto);
                }

                var pecaUpdated = await _pecaRepository.UpdateAsync(peca);
                if (!pecaUpdated)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<bool>.Failure("Falha ao atualizar peça");
                }

                //var orcamentosPeca = await _orcamentoPecaRepository.GetByPecaIdAsync(pecaId);

                //foreach (var orcamentoPeca in orcamentosPeca)
                //{
                //    var result = await _orcamentoPecaService.UsarPecaNoOrcamento(orcamentoPeca.OrcamentoId, pecaId);
                //    if (!result.IsSuccess)
                //    {
                //        await _unitOfWork.RollbackAsync();
                //        return Result<bool>.Failure("Erro ao usar peça no orçamento");
                //    }
                //}

                var movimentacao = new MovimentacaoEstoque
                {
                    PecaId = pecaId,
                    Quantidade = quantidadeReposicao,
                    Tipo = ETipoMovimentacao.Entrada
                };

                var registroResult = await _movimentacaoEstoqueRepository.RegistrarMovimentacaoAsync(movimentacao);
                if (!registroResult)
                {
                    await _unitOfWork.RollbackAsync();
                    return Result<bool>.Failure("Falha ao registrar movimentação");
                }

                await _unitOfWork.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await _unitOfWork.CommitAsync();
                return Result<bool>.Failure($"Erro inesperado: {ex.Message}");
            }
        }

    }
}
