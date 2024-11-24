using CP.Pagamentos.Domain.DomainObjects.Notifications;
using MediatR;

namespace CP.Pagamentos.Application.Notifications.Pagamentos;

public class PagamentoCriadoNotificationHandler : IRequestHandler<PagamentoCriado>
{

    public Task Handle(PagamentoCriado request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Pagamento criado: " + request.PagamentoId);
        throw new NotImplementedException();
    }
}
