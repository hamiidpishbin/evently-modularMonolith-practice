namespace Evently.Modules.Events.Domain.Abstractions;

public abstract class Entity
{
	protected Entity()
	{
		
	}

	private readonly List<IDomainEvent> _domainEvents = [];

	public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();
	
	public void ClearDomainEvents() => _domainEvents.Clear();
	
	public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}