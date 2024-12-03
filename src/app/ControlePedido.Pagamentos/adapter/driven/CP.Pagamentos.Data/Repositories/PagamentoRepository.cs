using System.Diagnostics.CodeAnalysis;
using CP.Pagamentos.Data.Mappings;
using CP.Pagamentos.Domain.Adapters.Repositories;
using CP.Pagamentos.Domain.Entities;

namespace CP.Pagamentos.Data.Repositories;

[ExcludeFromCodeCoverage]
public class PagamentoRepository : IPagamentoRepository
{
    private const string TABLE_NAME = "ControlePedidosPagamentos";
    private readonly PagamentoDynamoDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public PagamentoRepository(PagamentoDynamoDbContext context)
    {
        _context = context;
    }

    public void Criar(Pagamento pagamento)
    {
        _context.Add(new PagamentoDynamoDbMapping(pagamento), TABLE_NAME);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
