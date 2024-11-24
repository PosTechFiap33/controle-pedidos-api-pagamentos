using CP.Pagamentos.Api;
using CP.Pagamentos.Api.Configurations;
using CP.Pagamentos.Api.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CP.Pagamentos.IOC.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiConfiguration();

builder.Services.RegisterServices(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());
                 
var app = builder.Build();

app.UseSwaggerApp();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
