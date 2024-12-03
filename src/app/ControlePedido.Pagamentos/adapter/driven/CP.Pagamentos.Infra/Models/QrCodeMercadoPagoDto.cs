using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CP.Pagamentos.Infra.Models;

[ExcludeFromCodeCoverage]
public class QrCodeMercadoPagoDto
{
    [JsonPropertyName("in_store_order_id")]
    public string InStoreOrderId { get; set; }

    [JsonPropertyName("qr_data")]
    public string QrData { get; set; }
}

