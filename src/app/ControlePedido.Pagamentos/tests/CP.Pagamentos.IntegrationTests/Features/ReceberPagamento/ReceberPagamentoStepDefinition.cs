
using System;
using System.Net;
using System.Net.Http.Json;
using CP.Pagamentos.Api.DTOs;
using CP.Pagamentos.IntegrationTests;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace MyNamespace
{
    [Binding]
    public class ReceberPagamentoStepDefinition
    {
        private HttpResponseMessage _response;
        private readonly IntegrationTestFixture _fixture;
        private PagamentoMercadoPago _pagamento;

        public ReceberPagamentoStepDefinition(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Given(@"que criado os dados de pagamento")]
        public void Givenquecriadoosdadosdepagamento()
        {
            _pagamento = new PagamentoMercadoPago
            {
                Acao = "payment.created",
                Dados = new Dados
                {
                    TransacaoId = "Pagamento de teste"
                }
            };
        }

        [When(@"for feita uma requisicao para a rota de webhook")]
        public async Task Whenforfeitaumarequisicaoparaarotadewebhook()
        {
            _response = await _fixture.Client.PostAsJsonAsync($"MercadoPagoWebhook", _pagamento);
        }

        [Then(@"o status code da requisicao deve ser (.*)")]
        public void Thenostatuscodedarequisicaodeveser(int status)
        {
            _response.StatusCode.Should().Be((HttpStatusCode)status);
        }

    }
}