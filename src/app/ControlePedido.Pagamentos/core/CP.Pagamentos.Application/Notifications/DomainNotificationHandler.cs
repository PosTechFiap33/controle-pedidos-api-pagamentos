using System.Diagnostics.CodeAnalysis;
using CP.Pagamentos.Domain.DomainObjects;
using MediatR;

namespace CP.Pagamentos.Application.Notifications;

[ExcludeFromCodeCoverage]
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
