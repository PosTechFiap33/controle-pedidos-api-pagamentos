namespace CP.Pagamentos.Domain.Entities;

public class PedidoItem {
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}

public class Pedido
{
    public Guid Id {get; private set;}
    public decimal Valor { get; private set; }
    public ICollection<PedidoItem> Itens { get; private set; }

    public Pedido(Guid id, decimal valor, ICollection<PedidoItem> itens)
    {
        Id = id;
        Valor = valor;
        Itens = itens;
    }
}
