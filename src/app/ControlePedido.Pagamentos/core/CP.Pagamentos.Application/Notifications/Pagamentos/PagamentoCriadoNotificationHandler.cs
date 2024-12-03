using System.Diagnostics.CodeAnalysis;
using CP.Pagamentos.CrpssCutting.Configuration;
using CP.Pagamentos.Domain.Adapters.MessageBus;
using CP.Pagamentos.Domain.DomainObjects.Notifications;
using Microsoft.Extensions.Options;

using MediatR;

namespace CP.Pagamentos.Application.Notifications.Pagamentos;

[ExcludeFromCodeCoverage]
public class PagamentoCriadoNotificationHandler : IRequestHandler<PagamentoCriado>
{
    private readonly IMessageBus _messageBus;
    private readonly AWSConfiguration _awsConfiguration;

    public PagamentoCriadoNotificationHandler(IMessageBus messageBus,
                                              IOptions<AWSConfiguration> awsConfiguration)
    {
        _messageBus = messageBus;
        _awsConfiguration = awsConfiguration.Value;
    }

    public async Task Handle(PagamentoCriado request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Pagamento criado: " + request.PagamentoId);
        await _messageBus.PublishAsync(request, _awsConfiguration.PagamentoQueueUrl);
    }
}
