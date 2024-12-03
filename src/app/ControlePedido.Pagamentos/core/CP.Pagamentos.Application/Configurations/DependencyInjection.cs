using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CP.Pagamentos.Application.Notifications;
using CP.Pagamentos.Application.Notifications.Pagamentos;
using CP.Pagamentos.Application.UseCases.CriarQrCodePagamento;
using CP.Pagamentos.Application.UseCases.PagarPedido;
using CP.Pagamentos.Domain.DomainObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Pagamentos.Application.Configurations;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IPagarPedidoUseCase, PagarPedidoUseCaseHandler>();
        services.AddTransient<ICriarQrCodePagamentoUseCase, CriarQrCodePagamentoHandler>();

        services.AddScoped<IDomainNotificationEmmiter, DomainNotificationEmmiter>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.RegisterServicesFromAssembly(typeof(PagamentoCriadoNotificationHandler).Assembly);
        });

        return services;
    }
}
