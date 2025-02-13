using OficinaTech.Domain.Enum;

namespace OficinaTech.Domain.Entities
{
    public class OrcamentoPeca
    {
        public int Id { get; set; }
        public int OrcamentoId { get; set; }
        public Orcamento Orcamento { get; set; }
        public int PecaId { get; set; }
        public Peca Peca { get; set; }
        public int Quantidade { get; set; }
        public bool LiberadaParaCompra { get; set; }
        public EEstadoPecaOrcamento Status { get; set; } = EEstadoPecaOrcamento.EmEspera;

    }

}
