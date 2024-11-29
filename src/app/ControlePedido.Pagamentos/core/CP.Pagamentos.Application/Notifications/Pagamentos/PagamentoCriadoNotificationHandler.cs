using CP.Pagamentos.Domain.Adapters.MessageBus;
using CP.Pagamentos.Domain.DomainObjects.Notifications;
using MediatR;

namespace CP.Pagamentos.Application.Notifications.Pagamentos;

public class PagamentoCriadoNotificationHandler : IRequestHandler<PagamentoCriado>
{
    private readonly IMessageBus _messageBus;

    public PagamentoCriadoNotificationHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(PagamentoCriado request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Pagamento criado: " + request.PagamentoId);
        //TODO: melhorar essa parte
        await _messageBus.PublishAsync(request, "http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/ControlePedidosPagamentos");
    }
}
