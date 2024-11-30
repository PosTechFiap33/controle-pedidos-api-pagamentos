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
    /// Gera um QR Code para pagamento de um pedido específico.
    /// </summary>
    /// <remarks>
    /// Esta operação recebe os dados de pagamento do cliente e utiliza um caso de uso para gerar o QR Code necessário para o pagamento do pedido. 
    /// O QR Code gerado pode ser utilizado em sistemas de pagamento que suportam essa tecnologia para concluir a transação.
    /// </remarks>
    /// <param name="pagamento">Os dados de pagamento fornecidos pelo cliente, incluindo informações sobre os itens do pedido.</param>
    /// <param name="pedidoId">O identificador único do pedido para o qual o QR Code de pagamento será gerado.</param>
    /// <param name="useCase">A instância do caso de uso que executa a lógica para gerar o QR Code de pagamento.</param>
    /// <returns>Uma resposta personalizada contendo o resultado da geração do QR Code, incluindo os dados gerados e o status da operação.</returns>
    [HttpPost("{pedidoId}/qrcode")]
    [ProducesResponseType(typeof(CriarQrCodePagamentoUseCaseResult), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<CriarQrCodePagamentoUseCaseResult>> GerarQrcode([FromBody] GeracaoQrCode geracaoQrCode,
                                                                                   [FromRoute] Guid pedidoId,
                                                                                   [FromServices] ICriarQrCodePagamentoUseCase useCase)
    {
        _logger.LogInformation($"{nameof(PedidoController)} - {nameof(GerarQrcode)}: Iniciando geracao do qrcode para o pedido ${pedidoId}", geracaoQrCode);
       
        var response = await useCase.Executar(new CriarQrCodePagamentoUseCase(pedidoId, geracaoQrCode.Itens));
       
        return CustomResponse(response, HttpStatusCode.Created);
    }

}
