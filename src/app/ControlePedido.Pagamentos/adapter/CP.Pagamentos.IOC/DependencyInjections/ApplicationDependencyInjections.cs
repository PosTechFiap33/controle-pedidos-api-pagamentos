using CP.Pagamentos.Application.Configurations;
using CP.Pagamentos.Application.UseCases.PagarPedido;
using CP.Pagamentos.Data.Configuration;
using CP.Pagamentos.Infra.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Pagamentos.IOC.DependencyInjections;

public static class ApplicationDependencyInjections
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfraConfiguration(configuration);
        services.AddApplicationConfiguration();
        services.AddDatabaseConfiguration(configuration);
        services.AddTransient<IPagarPedidoUseCase, PagarPedidoUseCaseHandler>();
    }
}
