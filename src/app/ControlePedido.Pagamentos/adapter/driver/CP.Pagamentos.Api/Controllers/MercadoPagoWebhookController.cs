using System.Net;
using CP.Pagamentos.Api.DTOs;
using CP.Pagamentos.Application.UseCases.PagarPedido;
using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.Entities;
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
    /// Recebe os dados de pagamento e executa o caso de uso para realizar o pagamento.
    /// </remarks>
    /// <param name="pagamento">Os dados de pagamento fornecidos pelo cliente.</param>
    /// <param name="useCase">A instância do caso de uso para processar o pagamento.</param>
    /// <returns>Uma resposta customizada com o status da operação.</returns>
    [HttpPost]
    public async Task<IActionResult> Pagamento([FromBody] PagamentoMercadoPago pagamento, 
                                               [FromServices] IPagarPedidoUseCase useCase,
                                               [FromServices] IPagamentoRepository repositorio)
    {
        repositorio.Criar(new Pagamento(Guid.NewGuid(), 10, "teste", "local", DateTime.UtcNow));

        await repositorio.UnitOfWork.Commit();

        await useCase.Executar(new PagarPedidoUseCase(pagamento.Dados.TransacaoId));

        return CustomResponse(null, HttpStatusCode.Created);
    }
}