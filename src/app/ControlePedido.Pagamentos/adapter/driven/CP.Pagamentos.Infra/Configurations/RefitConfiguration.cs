using CP.Pagamentos.Domain.Adapters.Providers;
using CP.Pagamentos.Infra.Providers;
using CP.Pagamentos.Infra.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CP.Pagamentos.Infra.Configurations;

public static class RefitConfiguration
{
    public static IServiceCollection ConfigureHttpPayment(this IServiceCollection services)
    {
        services.AddRefitClient<MercadoPagoApi>()
               .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.mercadopago.com"));
        services.AddTransient<IPagamentoProvider, PagamentoMercadoPagoProvider>();
        return services;
    }
}