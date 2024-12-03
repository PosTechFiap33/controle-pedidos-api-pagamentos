using System.Diagnostics.CodeAnalysis;
using CP.Pagamentos.CrpssCutting.Configuration;
using CP.Pagamentos.Data.Repositories;
using CP.Pagamentos.Domain.Adapters.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Pagamentos.Data.Configuration;

[ExcludeFromCodeCoverage]

public static class DynamoDbConfiguration
{

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration){
        services.Configure<AWSConfiguration>(configuration.GetSection("AWS"));
        services.AddScoped<PagamentoDynamoDbContext>();
        services.AddTransient<IPagamentoRepository, PagamentoRepository> ();
        return services;
    }
}
