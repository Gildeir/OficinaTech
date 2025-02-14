namespace OficinaTech.Domain.Enum
{
    public enum EEstadoPecaOrcamento
    {
        EmEspera = 1,               // Peça adicionada ao orçamento, mas não disponível ainda
        LiberadaParaCompra = 2,     // Peça precisa ser comprada
        Comprada = 3,               // Peça já foi comprada
        Entregue = 4                // Peça já foi entregue ao orçamento
    }
}
