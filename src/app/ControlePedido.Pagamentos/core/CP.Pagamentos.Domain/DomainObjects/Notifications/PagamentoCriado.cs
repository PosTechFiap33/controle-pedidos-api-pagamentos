namespace CP.Pagamentos.Domain.DomainObjects.Notifications;

public class PagamentoCriado : IDomainNotification
{
    public Guid PagamentoId { get; private set; }
    public Guid PedidoId { get; private set; }

    public PagamentoCriado(Guid pagamentoId, Guid pedidoId)
    {
        PagamentoId = pagamentoId;
        PedidoId = pedidoId;
    }

}
