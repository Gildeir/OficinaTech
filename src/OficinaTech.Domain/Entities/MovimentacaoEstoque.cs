using OficinaTech.Domain.Enum;
using System;

namespace OficinaTech.Domain.Entities
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public int PecaId { get; set; }
        public Peca Peca { get; set; }
        public int Quantidade { get; set; }
        public ETipoMovimentacao Tipo { get; set; }
        public DateTime DataMovimentacao { get; set; } = DateTime.UtcNow;
    }

}
