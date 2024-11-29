using System.Net;
using CP.Pagamentos.Api.DTOs;
using CP.Pagamentos.Application.UseCases.CriarQrCodePagamento;
using CP.Pagamentos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CP.Pagamentos.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PedidoController : MainController
{

    public PedidoController(ILogger<PedidoController> logger) : base(logger)
    {
    }

    /// <summary>
    /// Gera um qrcode para pagamento do pedido  
    /// </summary>
    /// <remarks>
    /// Recebe os dados de pagamento e executa o caso de uso para realizar o pagamento.
    /// </remarks>
    /// <param name="pagamento">Os dados de pagamento fornecidos pelo cliente.</param>
    /// <param name="useCase">A instância do caso de uso para processar o pagamento.</param>
    /// <returns>Uma resposta customizada com o status da operação.</returns>
    [HttpPost("{pedidoId}/qrcode")]
    public async Task<ActionResult<CriarQrCodePagamentoUseCaseResult>> Pagamento([FromBody] GeracaoQrCode geracaoQrCode,
                                                                                 [FromRoute] Guid pedidoId,
                                                                                 [FromServices] ICriarQrCodePagamentoUseCase useCase)
    {
        var response = await useCase.Executar(new CriarQrCodePagamentoUseCase(pedidoId, geracaoQrCode.Itens));
        return CustomResponse(response, HttpStatusCode.Created);
    }

}
