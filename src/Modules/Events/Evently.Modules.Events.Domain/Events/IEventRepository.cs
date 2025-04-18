namespace Evently.Modules.Events.Domain.Events;

public interface IEventRepository
{
	void Insert(Event @event);
	ValueTask<Event?> GetByIdAsync(Guid id);
}