using MediatR;

namespace CP.Pagamentos.Domain.DomainObjects;

public class IDomainNotification : IRequest
{
}

public interface IDomainNotificationEmmiter
{
    Task SendNotification<T>(T notification) where T : IDomainNotification;
}
