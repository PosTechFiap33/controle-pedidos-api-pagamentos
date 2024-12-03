namespace CP.Pagamentos.Application.UseCases.PagarPedido;

public interface IPagarPedidoUseCase
{
    Task Executar(PagarPedidoUseCase pagarPedido);
}
