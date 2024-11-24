using CP.Pagamentos.Domain.DomainObjects;
using MediatR;

namespace CP.Pagamentos.Application.Notifications;

public class DomainNotificationEmmiter : IDomainNotificationEmmiter
{
    private readonly IMediator _mediator;

    public DomainNotificationEmmiter(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendNotification<T>(T notification) where T : IDomainNotification
    {
        await _mediator.Send(notification);
    }
}
