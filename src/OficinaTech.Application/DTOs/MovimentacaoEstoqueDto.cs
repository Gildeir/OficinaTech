using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaTech.Application.DTOs
{
    public class MovimentacaoEstoqueDto
    {
        public int Id { get; set; }
        public int PecaId { get; set; }
        public string NomePeca { get; set; }
        public int Quantidade { get; set; }
        public string Tipo { get; set; }
        public DateTime DataMovimentacao { get; set; }
    }

}
