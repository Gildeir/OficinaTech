using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Application.DTOs
{
    public class OrcamentoDto
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string Cliente { get; set; }
        public string PlacaVeiculo { get; set; }
        public List<PecaDto> Pecas { get; set; } = new();
    }

}
