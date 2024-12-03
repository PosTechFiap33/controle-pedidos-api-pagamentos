using System.Net;
using System.Net.Http.Json;
using CP.Pagamentos.Api.DTOs;
using CP.Pagamentos.Application.UseCases.CriarQrCodePagamento;
using CP.Pagamentos.Domain.Entities;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CP.Pagamentos.IntegrationTests.Features.GerarQrCode;

[Binding]
public class GerarQrCodeStepsDefinitions
{
    private GeracaoQrCode _geracaoQrCode;
    private HttpResponseMessage _response;
    private readonly IntegrationTestFixture _fixture;

    public GerarQrCodeStepsDefinitions(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _geracaoQrCode = new GeracaoQrCode();
    }

    [Given(@"que seja criada uma lista de itens para um pedido")]
    public void DadoQueSejaCriadaUmaListaDeItensParaUmPedido()
    {
        _geracaoQrCode.Itens = new List<PedidoItem>();
    }

    [Given(@"que a lista contenha um item com os seguintes dados:")]
    public void DadoQueAListaContenhaUmItemComOsSeguintesDados(Table table)
    {
        foreach (var row in table.Rows)
        {
            var item = new PedidoItem
            {
                Nome = row["Nome"],
                Descricao = row["Descrição"],
                Preco = decimal.Parse(row["Preço"]),
                Quantidade = int.Parse(row["Quantidade"])
            };
            _geracaoQrCode.Itens.Add(item);
        }
    }

    [When(@"uma requisição for enviada para a API de geração de QR Code")]
    public async Task QuandoUmaRequisicaoForEnviadaParaAApiDeGeracaoDeQRCode()
    {
        _response = await _fixture.Client.PostAsJsonAsync($"pedido/{Guid.NewGuid()}/qrcode", _geracaoQrCode);
    }

    [Then(@"o status da resposta deverá ser (.*) \(Created\)")]
    public void EntaoOStatusDaRespostaDeveraSerCreated(HttpStatusCode statusCode)
    {
        _response.StatusCode.Should().Be(statusCode);
    }

    [Then(@"a resposta deverá conter o QR Code gerado")]
    public async Task EntaoARespostaDeveraConterOQRCodeGerado()
    {
        var dadosPedido = await _fixture.RecuperarConteudo<CriarQrCodePagamentoUseCaseResult>(_response);
        dadosPedido.QrCode.Should().NotBeEmpty();
        dadosPedido.QrCode.Should().Contain("COM.MERCADOLIBRE");
    }
}