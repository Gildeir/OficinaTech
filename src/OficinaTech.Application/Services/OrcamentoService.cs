using OficinaTech.Application.Interfaces;
using OficinaTech.Domain.Entities;
using OficinaTech.Infrastructure.Repositories.Interfaces;

namespace OficinaTech.Application.Services
{
    public class OrcamentoService : IOrcamentoService
    {
        private readonly IOrcamentoRepository _orcamentoRepository;
        private readonly IPecaRepository _pecaRepository;

        public OrcamentoService(IOrcamentoRepository orcamentoRepository, IPecaRepository pecaRepository)
        {
            _orcamentoRepository = orcamentoRepository;
            _pecaRepository = pecaRepository;
        }

        public async Task<Orcamento> CriarOrcamentoAsync(string numero, string placa, string cliente)
        {
            var orcamento = new Orcamento
            {
                Numero = numero,
                PlacaVeiculo = placa,
                Cliente = cliente,
                OrcamentoPecas = new List<OrcamentoPeca>()
            };

            await _orcamentoRepository.AdicionarAsync(orcamento);
            return orcamento;
        }

        public async Task<List<Orcamento>> ObterTodosOrcamentosAsync()
        {
            return await _orcamentoRepository.ObterTodosAsync();
        }

        public async Task<Orcamento> ObterOrcamentoPorIdAsync(int id)
        {
            return await _orcamentoRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> AdicionarPecaAoOrcamentoAsync(int orcamentoId, int pecaId, int quantidade)
        {
            var orcamento = await _orcamentoRepository.ObterPorIdAsync(orcamentoId);
            if (orcamento == null) return false;

            var peca = await _pecaRepository.GetByIdAsync(pecaId);
            if (peca == null) return false;

            var pecaOrcamento = new OrcamentoPeca
            {
                OrcamentoId = orcamentoId,
                PecaId = pecaId,
                Quantidade = quantidade,
                LiberadaParaCompra = peca.Estoque < quantidade
            };

            orcamento.OrcamentoPecas.Add(pecaOrcamento);
            await _orcamentoRepository.AtualizarAsync(orcamento);

            return true;
        }
    }

}
