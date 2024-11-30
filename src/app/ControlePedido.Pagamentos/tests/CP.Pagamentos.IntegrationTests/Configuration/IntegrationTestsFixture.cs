using System.Text.Json;
using CP.Pagamentos.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

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

                // builder.ConfigureServices(async services =>
                // {
                //     // Remove o contexto de banco de dados existente, se houver
                //     // var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(PagamentoDynamoDbContext));
                //     // if (descriptor != null)
                //     // {
                //     //     services.Remove(descriptor);
                //     // }
                // });
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

