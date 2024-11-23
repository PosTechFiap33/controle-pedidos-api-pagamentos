using CP.Pagamentos.Application.UseCases.PagarPedido;
using Microsoft.Extensions.DependencyInjection;

namespace CP.Pagamentos.IOC.DependencyInjections;

public static class ApplicationDependencyInjections
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IPagarPedidoUseCase, PagarPedidoUseCaseHandler>();
    }
}
