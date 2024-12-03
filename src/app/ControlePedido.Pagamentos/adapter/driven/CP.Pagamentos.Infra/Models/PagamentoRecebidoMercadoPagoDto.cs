using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CP.Pagamentos.Infra.Models;

[ExcludeFromCodeCoverage]
public class PagamentoRecebidoMercadoPagoDto
{
    [JsonPropertyName("id")]
    public long TransacaoId { get; set; }

    [JsonPropertyName("external_reference")]
    public string PedidoId { get; set; }

    [JsonPropertyName("date_approved")]
    public DateTime DataHoraPagamento { get; set; }

    [JsonPropertyName("transaction_amount")]
    public decimal ValorPago { get; set; }
}
