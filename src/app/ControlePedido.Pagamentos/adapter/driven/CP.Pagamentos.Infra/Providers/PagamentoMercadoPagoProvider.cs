using System.Diagnostics.CodeAnalysis;
using CP.Pagamentos.Domain.Adapters.Providers;
using CP.Pagamentos.Domain.DomainObjects;
using CP.Pagamentos.Domain.Entities;
using CP.Pagamentos.Domain.Models;
using CP.Pagamentos.Infra.Configurations;
using CP.Pagamentos.Infra.Extensions;
using CP.Pagamentos.Infra.Models;
using CP.Pagamentos.Infra.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

namespace CP.Pagamentos.Infra.Providers;

[ExcludeFromCodeCoverage]
public class PagamentoMercadoPagoProvider : IPagamentoProvider
{
    private readonly MercadoPagoApi _mercadoPagoApi;
    private readonly MercadoPagoIntegration _integration;
    private readonly ILogger<PagamentoMercadoPagoProvider> _logger;

    public PagamentoMercadoPagoProvider(ILogger<PagamentoMercadoPagoProvider> logger,
                                        MercadoPagoApi mercadoPagoApi,
                                        IOptions<MercadoPagoIntegration> integration)
    {
        _mercadoPagoApi = mercadoPagoApi;
        _integration = integration.Value;
        _logger = logger;
    }

    public async Task<string> GerarQRCodePagamento(Pedido pedido)
    {
        var mensagemErro = "Não foi possível comunicar com o sistema de pagamento para gerar o qrcode, utilize a rota de pagamento manual para prosseguir com seu pedido!";

        try
        {
            _logger.LogInformation("Iniciando o processo de geração de QR Code para o pedido: " + pedido.Id);

            var token = _integration.Token;

            var urlWebhook = _integration.UrlWebhook;

            long userId = _integration.UserId;

            var pedidoDto = new PedidoMercadoPagoDto(pedido, urlWebhook);

            var result = await _mercadoPagoApi.GerarQrCode(token, pedidoDto, userId, _integration.ExternalPosId);

            _logger.LogInformation("QR Code gerado com sucesso para o pedido: " + pedido.Id);

            return result.QrData;
        }
        catch (ApiException apiEx)
        {
            _logger.LogApiError(apiEx, "Erro de API ao comunicar com o Mercado Pago");
            throw new ApplicationException(mensagemErro);
        }
    }

    public async Task<PagamentoRealizado> ValidarTransacao(string idTransacao)
    {
        var mensagemErro = "Ocorreu um erro interno ao comunicar com o sistema de pagamento!";

        try
        {
            var pagamento = await _mercadoPagoApi.ConsultarPagamento(_integration.Token, idTransacao);
            return new PagamentoRealizado(Guid.Parse(pagamento.PedidoId), 
                                          pagamento.DataHoraPagamento, 
                                          pagamento.TransacaoId.ToString(), 
                                          pagamento.ValorPago, "Mercado Pago");
        }
        catch (ApiException apiEx)
        {
            _logger.LogApiError(apiEx, "Erro de API ao comunicar com o Mercado Pago");
            throw new IntegrationExceptions(mensagemErro);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ocorreu um erro ao comunicar com o Mercado Pago: {ErrorMessage}.", e.Message);
            throw new IntegrationExceptions(mensagemErro);
        }
    }
}
