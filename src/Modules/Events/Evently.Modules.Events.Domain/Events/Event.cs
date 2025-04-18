using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Domain.Events;

public sealed class Event : Entity
{
	public Guid Id { get; private set; }
	public string Title { get; private set; } = null!;
	public string Description { get; private set; } = null!;
	public string Location { get; private set; } = null!;
	public DateTime StartsAtUtc { get; private set; }
	public DateTime? EndsAtUtc { get; private set; }
	public EventStatus Status { get; private set; }

	private Event()
	{
		
	}

	public static Event Create(string title, string description, string location, DateTime startsAtUtc, DateTime? endsAtUtc)
	{
		var @event = new Event()
		{
			Id = Guid.NewGuid(),
			Title = title,
			Description = description,
			Location = location,
			StartsAtUtc = startsAtUtc,
			EndsAtUtc = endsAtUtc,
			Status = EventStatus.Draft
		};
		
		@event.RaiseDomainEvent(new EventCreatedDomainEvent(@event.Id));
		
		return @event;
	}
}

public sealed class EventCreatedDomainEvent(Guid eventId) : DomainEvent
{
	public Guid EventId { get; init; } = eventId; 
}