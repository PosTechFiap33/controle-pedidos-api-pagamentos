using CP.Pagamentos.Domain.Adapters.Providers;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Application.UseCases.CriarQrCodePagamento;

public class CriarQrCodePagamentoHandler : ICriarQrCodePagamentoUseCase
{
    private readonly IPagamentoProvider _provider;

    public CriarQrCodePagamentoHandler(IPagamentoProvider provider)
    {
        _provider = provider;
    }

    public async Task<CriarQrCodePagamentoUseCaseResult> Executar(CriarQrCodePagamentoUseCase dadosPagamento)
    {
        var valor = dadosPagamento.Itens.Sum(x => x.Preco * x.Quantidade);
        var pedido = new Pedido(dadosPagamento.PedidoId, valor, dadosPagamento.Itens);
        var qrCode = await _provider.GerarQRCodePagamento(pedido);
        return new CriarQrCodePagamentoUseCaseResult(qrCode);
    }
}
