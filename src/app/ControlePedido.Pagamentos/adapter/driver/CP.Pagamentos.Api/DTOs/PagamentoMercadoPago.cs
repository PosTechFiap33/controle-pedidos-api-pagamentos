using System.Text.Json.Serialization;

namespace CP.Pagamentos.Api.DTOs;

public class PagamentoMercadoPago
{
    [JsonPropertyName("action")]
    public string Acao { get; set; }

    [JsonPropertyName("data")]
    public Dados Dados { get; set; }
}

public class Dados
{
    [JsonPropertyName("id")]
    public string TransacaoId { get; set; }
}