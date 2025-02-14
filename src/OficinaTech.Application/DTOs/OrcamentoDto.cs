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
        public string Numero { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string PlacaVeiculo { get; set; } = string.Empty;
        public List<PecaDto> Pecas { get; set; } = new();
    }

}
