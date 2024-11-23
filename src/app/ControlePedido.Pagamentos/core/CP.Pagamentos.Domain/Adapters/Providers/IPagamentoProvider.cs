using CP.Pagamentos.Domain.Entities;
using CP.Pagamentos.Domain.Models;

namespace CP.Pagamentos.Domain.Adapters.Providers;

public interface IPagamentoProvider
{
    Task<string> GerarQRCodePagamento(Pedido pedido);
    Task<PagamentoRealizado> ValidarTransacao(string codigoTransacao);
}
