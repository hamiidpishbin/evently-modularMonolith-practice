using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure.Database;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
	public Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	public void Insert(Event @event)
	{
		context.Events.Add(@event);
	}
	
	public ValueTask<Event?> GetByIdAsync(Guid id)
	{
		return context.Events.FindAsync(id);
	}
}