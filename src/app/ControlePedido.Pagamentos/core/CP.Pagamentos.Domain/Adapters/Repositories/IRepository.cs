namespace CP.Pagamentos.Domain.Adapters.Repositories;

public interface IAggregateRoot
{
}

public interface IUnitOfWork : IDisposable
{
    Task<bool> Commit();
}

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
