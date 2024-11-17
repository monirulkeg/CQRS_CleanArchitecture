using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;


public abstract class BaseEntity<T> : IBaseEntity
{
    // Using generic type T for Id to support different key types
    public T Id { get; set; } = default!;

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public interface IBaseEntity
{
    IReadOnlyCollection<BaseEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
