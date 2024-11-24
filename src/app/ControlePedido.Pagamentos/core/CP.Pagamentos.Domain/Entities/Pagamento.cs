using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.DomainObjects;
using CP.Pagamentos.Domain.DomainObjects.Notifications;

namespace CP.Pagamentos.Domain.Entities;

public class Pagamento : Entity, IAggregateRoot
{
    public Guid PedidoId { get; private set; }
    public decimal ValorPago { get; private set; }
    public string CodigoTransacao { get; private set; }
    public string Provedor { get; private set; }
    public DateTime DataPagamento { get; private set; }

    public Pagamento(Guid pedidoId, decimal valorPago, string codigoTransacao, string provedor, DateTime dataPagamento)
    {
        PedidoId = pedidoId;
        ValorPago = valorPago;
        CodigoTransacao = codigoTransacao;
        Provedor = provedor;
        DataPagamento = dataPagamento;
        ValidateEntity();
        AddNotification(new PagamentoCriado(Id, PedidoId));
    }

    private void ValidateEntity()
    {
        AssertionConcern.AssertArgumentNotEquals(PedidoId, Guid.Empty, "O código do pedido deve ser informado!");
        AssertionConcern.AssertGratherThanValue(ValorPago, 0, "O valor pago deve ser superior a 0!");
        AssertionConcern.AssertArgumentNotEmpty(CodigoTransacao, "O código de transação não pode estar vazio!");
        AssertionConcern.AssertArgumentNotEmpty(Provedor, "O provedor não pode estar vazio!");
        AssertionConcern.AssertArgumentNotEquals(DataPagamento, DateTime.MinValue, "A data do pagamento deve ser informada!");
    }
}
