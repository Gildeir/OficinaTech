namespace OficinaTech.Domain.Entities
{
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Estoque { get; set; }
        public decimal Preco { get; set; }
        public List<OrcamentoPeca> OrcamentoPecas { get; set; } = new List<OrcamentoPeca>();

    }

}
