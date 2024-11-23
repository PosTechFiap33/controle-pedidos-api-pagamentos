namespace CP.Pagamentos.Domain.Entities;

public class PedidoItem : Entity {
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}

public class Pedido : Entity
{
    public decimal Valor { get; set; }
    public ICollection<PedidoItem> Itens { get; set; }
}
