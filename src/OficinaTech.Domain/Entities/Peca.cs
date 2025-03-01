using System.Text.Json.Serialization;

namespace OficinaTech.Domain.Entities
{
    public class Peca
    {
        public int Id { get; set; }
        public string Nome { get; set; } = String.Empty;
        public int Estoque { get; set; }
        public decimal Preco { get; set; }

        [JsonIgnore]
        public List<OrcamentoPeca> OrcamentoPecas { get; set; } = new List<OrcamentoPeca>();

        public List<MovimentacaoEstoque> MovimentacoesEstoque { get; set; } = new List<MovimentacaoEstoque>();

    }

}
