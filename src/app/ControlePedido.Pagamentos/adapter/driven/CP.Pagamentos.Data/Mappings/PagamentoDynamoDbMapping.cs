using Amazon.DynamoDBv2.Model;
using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Data.Mappings;

public interface IDynamoEntity<T> where T : Entity, IAggregateRoot
{
    T Entity { get; }
    public Dictionary<string, AttributeValue> MapToDynamo();
}

public class PagamentoDynamoDbMapping : IDynamoEntity<Pagamento>
{
    public Pagamento Entity { private set; get; }

    public PagamentoDynamoDbMapping(Pagamento pagamento)
    {
        Entity = pagamento;
    }

    public Dictionary<string, AttributeValue> MapToDynamo()
    {
        return new Dictionary<string, AttributeValue>{
            { nameof(Entity.Id), new AttributeValue {  S = Entity.Id.ToString() }},
            { nameof(Entity.PedidoId), new AttributeValue {  S = Entity.PedidoId.ToString() }},
            { nameof(Entity.ValorPago), new AttributeValue { N = Entity.ValorPago.ToString()} },
            { nameof(Entity.CodigoTransacao), new AttributeValue {S = Entity.CodigoTransacao}},
            { nameof(Entity.Provedor), new AttributeValue{S = Entity.Provedor}},
            { nameof(Entity.DataPagamento), new AttributeValue{S = Entity.DataPagamento.ToString()} },
        };
    }
}
