using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OficinaTech.Domain.Entities;

namespace OficinaTech.Application.Factories
{
    public static class OrcamentoFactory
    {
        public static Orcamento CriarOrcamento(string numero, string placa, string cliente)
        {
            return new Orcamento
            {
                Numero = numero,
                PlacaVeiculo = placa,
                Cliente = cliente,
                OrcamentoPecas = new List<OrcamentoPeca>()
            };
        }
    }

}
