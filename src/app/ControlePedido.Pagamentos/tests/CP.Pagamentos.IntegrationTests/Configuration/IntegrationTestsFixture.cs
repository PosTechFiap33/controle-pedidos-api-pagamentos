using System.Text.Json;
using CP.Pagamentos.Api.Services;
using CP.Pagamentos.Data;
using CP.Pagamentos.Domain.Adapters.Providers;
using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.Entities;
using CP.Pagamentos.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CP.Pagamentos.IntegrationTests;

public class IntegrationTestFixture : IDisposable
{
    public WebApplicationFactory<Program> _factory { get; private set; }
    public HttpClient Client { get; }
    public PagamentoDynamoDbContext context { get; private set; }

    public IntegrationTestFixture()
    {
        _factory = new WebApplicationFactory<Program>()
         .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                   {
                       context.HostingEnvironment.EnvironmentName = "Testing";
                   });

                builder.ConfigureServices(async services =>
                 {
                     services.AdicionarMocks();
                 });
            });

        Client = _factory.CreateClient();
    }

    public async Task TestarRequisicaoComErro(HttpResponseMessage response, List<string> erros)
    {
        var dados = await response.Content.ReadAsStringAsync();
        var errorDetail = JsonSerializer.Deserialize<ValidationProblemDetails>(dados);

        new ValidationProblemDetails(new Dictionary<string, string[]> {
                { "Mensagens", erros.ToArray() }
            });

        errorDetail.Errors["Mensagens"].Should().BeEquivalentTo(erros);
    }

    public async Task<T> RecuperarConteudo<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
    }

    public void Dispose()
    {
        Client.Dispose();
        _factory.Dispose();
    }
}

public static class FixtureExtensions
{
    public static IServiceCollection AdicionarMocks(this IServiceCollection services)
    {
        RemoverServicoInjetado<IPagamentoRepository>(services);
        var pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
        pagamentoRepositoryMock.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
        services.AddScoped(s => pagamentoRepositoryMock.Object);

        RemoverServicoInjetado<IPagamentoProvider>(services);
        var pagamentoProviderMock = new Mock<IPagamentoProvider>();
        pagamentoProviderMock.Setup(x => x.ValidarTransacao(It.IsAny<string>()))
                             .ReturnsAsync(new PagamentoRealizado(Guid.NewGuid(), DateTime.Now, "teste", 10, "Testes integrados"));
        pagamentoProviderMock.Setup(x => x.GerarQRCodePagamento(It.IsAny<Pedido>()))
                             .ReturnsAsync("00020101021243650016COM.MERCADOLIBRE020130636e5ad12fa-79be-4c57-b016-f5092fc9ed3e5204000053039865802BR5909Test Test6009SAO PAULO62070503***63046A2E");
        services.AddScoped(s => pagamentoProviderMock.Object);

        var paymentAuthorization = new Mock<IPaymentProviderRequestAuthorization>();
        paymentAuthorization.Setup(x => x.IsValid(It.IsAny<HttpRequest>())).Returns(true);
        RemoverServicoInjetado<IPaymentProviderRequestAuthorization>(services);
        services.AddScoped(s => paymentAuthorization.Object);

        return services;
    }

    public static IServiceCollection RemoverServicoInjetado<T>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
        return services;
    }
}