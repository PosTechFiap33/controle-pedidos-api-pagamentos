using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Domain.Models;

public class PagamentoRealizado
{
    public Guid PedidoId { get; private set; }
    public DateTime DataPagamento { get; private set; }
    public string CodigoTransacao { get; private set; }
    public decimal ValorPago { get; private set; }
    public string Provedor { get; private set; }

    public PagamentoRealizado(Guid pedidoId, DateTime dataPagamento, string codigoTransacao, decimal valorPago, string provedor)
    {
        PedidoId = pedidoId;
        DataPagamento = dataPagamento;
        CodigoTransacao = codigoTransacao;
        ValorPago = valorPago;
        Provedor = provedor;
    }

    public Pagamento GerarPagamento()
    {
        return new Pagamento(PedidoId, ValorPago, CodigoTransacao, Provedor, DataPagamento);
    }
}
