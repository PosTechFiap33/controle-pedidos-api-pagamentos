using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace CP.Pagamentos.Domain.DomainObjects;

[ExcludeFromCodeCoverage]
public class IDomainNotification : IRequest
{
}

public interface IDomainNotificationEmmiter
{
    Task SendNotification<T>(T notification) where T : IDomainNotification;
}
