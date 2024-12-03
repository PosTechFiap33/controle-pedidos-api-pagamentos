using System.Diagnostics.CodeAnalysis;
using CP.Pagamentos.Application.Configurations;
using CP.Pagamentos.Data.Configuration;
using CP.Pagamentos.Infra.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Pagamentos.IOC.DependencyInjections;

[ExcludeFromCodeCoverage]
public static class ApplicationDependencyInjections
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfraConfiguration(configuration);
        services.AddApplicationConfiguration();
        services.AddDatabaseConfiguration(configuration);
    }
}
