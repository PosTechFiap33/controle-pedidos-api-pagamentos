using CP.Pagamentos.Data.Mappings;
using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Data.Repositories;

public class PagamentoRepository : IPagamentoRepository
{
    private const string TABLE_NAME = "Pagamentos";
    private readonly PagamentoDynamoDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public PagamentoRepository(PagamentoDynamoDbContext context)
    {
        _context = context;
    }

    public void Criar(Pagamento pagamento)
    {
        _context.Add(pagamento.MapToDynamo(), TABLE_NAME);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
