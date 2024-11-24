using Amazon.DynamoDBv2.Model;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Data.Mappings;

public static class PagamentoDynamoDbMapping
{
    public static Dictionary<string, AttributeValue> MapToDynamo(this Pagamento pagamento)
    {
        return new Dictionary<string, AttributeValue>{
            { nameof(pagamento.Id), new AttributeValue {  S = pagamento.Id.ToString() }},
            { nameof(pagamento.PedidoId), new AttributeValue {  S = pagamento.PedidoId.ToString() }},
            { nameof(pagamento.ValorPago), new AttributeValue { N = pagamento.ValorPago.ToString()} },
            { nameof(pagamento.CodigoTransacao), new AttributeValue {S = pagamento.CodigoTransacao}},
            { nameof(pagamento.Provedor), new AttributeValue{S = pagamento.Provedor}},
            { nameof(pagamento.DataPagamento), new AttributeValue{S = pagamento.DataPagamento.ToString()} },
        };
    }
}
