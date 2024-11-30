using System.Text.Json.Serialization;
using CP.Pagamentos.Api.Middlewares;
using CP.Pagamentos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CP.Pagamentos.Api.Configurations;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddLogging(config =>
        {
            config.AddConsole();
            config.AddDebug();
        });

        services.AddScoped<IPagamentoApiRequestAuthorization, MercadoPagoApiService>();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddControllers(options =>
            options.Filters.Add<CustomModelStateValidationFilter>()
        ).AddJsonOptions(options =>
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddCors(option =>
        {
            option.AddPolicy("Total",
                builder =>
                  builder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                );
        });

        return services;
    }
}
