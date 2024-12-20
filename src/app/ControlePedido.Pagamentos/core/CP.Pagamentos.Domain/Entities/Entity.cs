using System.Diagnostics.CodeAnalysis;
using CP.Pagamentos.Domain.DomainObjects;

namespace CP.Pagamentos.Domain.Entities;

[ExcludeFromCodeCoverage]
public abstract class Entity
{
    public IDomainNotification Notification { get; private set; }
    
    public Guid Id { get; set; }

    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public void AddNotification(IDomainNotification notification){
        Notification = notification;
    }

    public override bool Equals(object obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo))
            return true;

        if (ReferenceEquals(null, compareTo))
            return false;

        return Id.Equals(compareTo.Id);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 997) + Id.GetHashCode();
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

}
