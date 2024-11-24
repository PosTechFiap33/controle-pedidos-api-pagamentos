using System.Reflection;
using CP.Pagamentos.Application.Notifications;
using CP.Pagamentos.Application.Notifications.Pagamentos;
using CP.Pagamentos.Domain.DomainObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Pagamentos.Application.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IDomainNotificationEmmiter, DomainNotificationEmmiter>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.RegisterServicesFromAssembly(typeof(PagamentoCriadoNotificationHandler).Assembly);
        });

        return services;
    }
}
