using System.ComponentModel;

namespace CP.Pagamentos.Application.UseCases.PagarPedido;

[DisplayName("PagarPedido")]
public class PagarPedidoUseCase
{
    public string CodigoTransacao { get; private set; }

    public PagarPedidoUseCase(string codigoTransacao)
    {
        CodigoTransacao = codigoTransacao;
    }
}