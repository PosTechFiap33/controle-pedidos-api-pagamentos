using CP.Pagamentos.Domain.Adapters.MessageBus;
using CP.Pagamentos.Domain.Adapters.Providers;
using CP.Pagamentos.Infra.Messaging;
using CP.Pagamentos.Infra.Providers;
using CP.Pagamentos.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CP.Pagamentos.Infra.Configurations;

public static class RefitConfiguration
{
    public static IServiceCollection AddInfraConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MercadoPagoIntegration>(configuration.GetSection("MercadoPagoIntegration"));

        services.AddRefitClient<MercadoPagoApi>()
               .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.mercadopago.com"));
       
       services.AddScoped<IMessageBus, SqsMessageBus>();

        services.AddTransient<IPagamentoProvider, PagamentoMercadoPagoProvider>();
       
        return services;
    }
}