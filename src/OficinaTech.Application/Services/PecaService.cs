using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Domain.Enum;
using OficinaTech.Infrastructure.Repositories;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class PecaService : IPecaService
    {
        private readonly IPecaRepository _pecaRepository;
        private readonly IOrcamentoPecaService _orcamentoPecaService;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;
        private readonly IOrcamentoPecaRepository _orcamentoPecaRepository;

        public PecaService(IPecaRepository pecaRepository, IOrcamentoPecaService orcamentoPecaService, 
            IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository, IOrcamentoPecaRepository orcamentoPecaRepository)
        {
            _pecaRepository = pecaRepository;
            _orcamentoPecaService = orcamentoPecaService;
            _movimentacaoEstoqueRepository = movimentacaoEstoqueRepository;
            _orcamentoPecaRepository = orcamentoPecaRepository;
        }
        public async Task<List<Peca>> GetAllPecasAsync()
        {
            return await _pecaRepository.GetAllAsync();
        }

        public async Task<Peca> GetPecaByIdAsync(int id)
        {
            return await _pecaRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddPecaAsync(Peca peca)
        {
            return await _pecaRepository.AddAsync(peca);
        }

        public async Task<bool> UpdatePecaAsync(Peca peca)
        {
            return await _pecaRepository.UpdateAsync(peca);
        }
        public async Task<bool> DeletePecaAsync(int id)
        {
            return await _pecaRepository.DeleteAsync(id);
        }

        public async Task<bool> ComprarPecaAsync(int pecaId, int quantidadeReposicao, decimal precoCusto)
        {
            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null) return false;

           peca.Estoque += quantidadeReposicao;

            
            if (peca.Preco < precoCusto)
            {
                peca.Preco = precoCusto;
                await _orcamentoPecaService.UpdatePrecoEmOrcamentos(pecaId, precoCusto);
            }

            await _pecaRepository.UpdateAsync(peca);

            var orcamentosPeca = await _orcamentoPecaRepository.GetByPecaIdAsync(pecaId);

            foreach (var orcamentoPeca in orcamentosPeca)
            {
                
                await _orcamentoPecaService.UsarPecaNoOrcamento(orcamentoPeca.OrcamentoId, pecaId);
            }

            var movimentacao = new MovimentacaoEstoque
            {
                PecaId = pecaId,
                Quantidade = quantidadeReposicao,
                Tipo = ETipoMovimentacao.Entrada
            };

            return await _movimentacaoEstoqueRepository.RegistrarMovimentacaoAsync(movimentacao);
        }



    }
}
