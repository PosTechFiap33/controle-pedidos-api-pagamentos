using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Domain.Models;

public class PagamentoRealizado
{
    public Guid PedidoId { get; set; }
    public DateTime DataPagamento { get; set; }
    public string CodigoTransacao { get; set; }
    public decimal ValorPago { get; set; }
    public string Provedor { get; set; }

    public Pagamento GerarPagamento()
    {
        return new Pagamento(PedidoId, ValorPago, CodigoTransacao, Provedor, DataPagamento);
    }
}
