using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Infra.Models;

[ExcludeFromCodeCoverage]
public class IntegradorMercadoPagoDto
{
    [JsonPropertyName("id")]
    public long UserId { get; set; }
}

[ExcludeFromCodeCoverage]
public class ItemMercadoPagoDto
{

    [JsonPropertyName("title")]
    public string Titulo { get; set; }

    [JsonPropertyName("description")]
    public string Descricao { get; set; }

    [JsonPropertyName("unit_price")]
    public decimal PrecoUnitario { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantidade { get; set; }

    [JsonPropertyName("unit_measure")]
    public string UnidadeMedida { get; set; }

    [JsonPropertyName("total_amount")]
    public decimal QuantiaTotal { get; set; }
}

public class SaqueMercadoPagoDto
{
    [JsonPropertyName("amount")]
    public decimal Quantia { get; set; }

    public SaqueMercadoPagoDto()
    {
        Quantia = 0;
    }
}

public class PedidoMercadoPagoDto
{
    [JsonPropertyName("cash_out")]
    public SaqueMercadoPagoDto Saque { get; private set; }

    [JsonPropertyName("description")]
    public string Descricao { get; set; }

    [JsonPropertyName("external_reference")]
    public string ReferenciaExterna { get; set; }

    [JsonPropertyName("items")]
    public List<ItemMercadoPagoDto> Itens { get; set; }

    [JsonPropertyName("notification_url")]
    public string UrlNotificacao { get; set; }

    [JsonPropertyName("sponsor")]
    public IntegradorMercadoPagoDto Patrocinador { get; set; }

    [JsonPropertyName("title")]
    public string Titulo { get; set; }

    [JsonPropertyName("total_amount")]
    public decimal QuantiaTotal { get; set; }

    public PedidoMercadoPagoDto(Pedido pedido, string urlNotificacao)
    {
        Saque = new SaqueMercadoPagoDto();
        Descricao = $"Pedido número {pedido.Id}";
        ReferenciaExterna = pedido.Id.ToString();
        Itens = pedido.Itens.Select(i => new ItemMercadoPagoDto
        {
            Titulo = i.Nome,
            Descricao = i.Descricao,
            PrecoUnitario = i.Preco,
            Quantidade = i.Quantidade,
            UnidadeMedida = "unit",
            QuantiaTotal = i.Preco * i.Quantidade
        }).ToList();
        UrlNotificacao = urlNotificacao;
        Titulo = "Pagamento para controle de pedido";
        QuantiaTotal = pedido.Valor;
    }
}