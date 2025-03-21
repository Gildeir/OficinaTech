﻿using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace OficinaTech.Domain.Entities
{
    public class Orcamento
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string PlacaVeiculo { get; set; }
        public string Cliente { get; set; }
        public List<OrcamentoPeca> OrcamentoPecas { get; set; } = new List<OrcamentoPeca>();
        public ServiceOrder ServiceOrder { get; set; }
        public decimal Total { get; private set; }
        public void CalculaTotal()
        {
            Total = 0;

            foreach (var item in OrcamentoPecas)
            {
                Total += item.Quantidade * item.Peca.Preco;  
            }
        }
    }

}
