using System.Net;
using CP.Pagamentos.Api.DTOs;
using CP.Pagamentos.Api.Services;
using CP.Pagamentos.Application.UseCases.PagarPedido;
using Microsoft.AspNetCore.Mvc;

namespace CP.Pagamentos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MercadoPagoWebhookController : MainController
{
    public MercadoPagoWebhookController(ILogger<MercadoPagoWebhookController> logger) : base(logger)
    {
    }

    /// <summary>
    /// Processa um pagamento utilizando o Mercado Pago.
    /// </summary>
    /// <remarks>
    /// Este endpoint recebe os dados de um pagamento e executa o caso de uso associado para realizar a operação.
    /// Ele também valida a autorização do request e verifica se a ação recebida é suportada.
    /// </remarks>
    /// <param name="pagamento">Os dados do pagamento enviados no corpo da requisição.</param>
    /// <param name="requestAuthorization">Serviço responsável por validar a autorização da requisição.</param>
    /// <param name="useCase">Caso de uso para processamento do pagamento.</param>
    /// <returns>
    /// Retorna:
    /// - **201 Created**: Se o pagamento for processado com sucesso.
    /// - **401 Unauthorized**: Se a autorização falhar.
    /// - **200 OK**: Se a ação do evento não for suportada ou não mapeada.
    /// </returns>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Pagamento(
        [FromBody] PagamentoMercadoPago pagamento,
        [FromServices] IPaymentProviderRequestAuthorization requestAuthorization,
        [FromServices] IPagarPedidoUseCase useCase)
    {
        _logger.LogInformation(
            "{Controller} - {Action}: Iniciando processamento do pagamento com os dados: {@Pagamento}",
            nameof(MercadoPagoWebhookController),
            nameof(Pagamento),
            pagamento);

        if (!requestAuthorization.IsValid(Request))
        {
            AdicionarErroProcessamento("Requisição não autorizada.");
            return CustomResponse(statusCode: HttpStatusCode.Unauthorized);
        }

        if (pagamento.Acao != "payment.created")
        {
            _logger.LogInformation(
                "{Controller} - {Action}: Ação não mapeada: {Acao}.",
                nameof(MercadoPagoWebhookController),
                nameof(Pagamento),
                pagamento.Acao);

            return CustomResponse("Evento informado não mapeado");
        }

        await useCase.Executar(new PagarPedidoUseCase(pagamento.Dados.TransacaoId));

        _logger.LogInformation(
            "{Controller} - {Action}: Pagamento processado com sucesso para TransacaoId: {TransacaoId}.",
            nameof(MercadoPagoWebhookController),
            nameof(Pagamento),
            pagamento.Dados.TransacaoId);

        return CustomResponse(statusCode: HttpStatusCode.Created);
    }
}
