using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Domain.Adapters.Repositories;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    void Criar(Pagamento pagamento);
}
