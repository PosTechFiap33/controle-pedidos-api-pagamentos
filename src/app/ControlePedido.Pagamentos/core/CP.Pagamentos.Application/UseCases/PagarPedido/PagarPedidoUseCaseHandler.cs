using CP.Pagamentos.Domain.Adapters.Providers;
using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.DomainObjects;

namespace CP.Pagamentos.Application.UseCases.PagarPedido;

public class PagarPedidoUseCaseHandler : IPagarPedidoUseCase
{
    private readonly IPagamentoRepository _repository;
    private readonly IPagamentoProvider _pagamentoProvider;

    public PagarPedidoUseCaseHandler(IPagamentoRepository repository,
                                     IPagamentoProvider pagamentoProvider)
    {
        _repository = repository;
        _pagamentoProvider = pagamentoProvider;
    }

    public async Task Executar(PagarPedidoUseCase pagarPedido)
    {
        var pagamentoRealizado = await _pagamentoProvider.ValidarTransacao(pagarPedido.CodigoTransacao);

        if (pagamentoRealizado == null)
            throw new DomainException("Não foi encontrado um pagamento para o código de transação informado!");

        var pagamento = pagamentoRealizado.GerarPagamento();

        _repository.Criar(pagamento);

        await _repository.UnitOfWork.Commit();
    }
}
