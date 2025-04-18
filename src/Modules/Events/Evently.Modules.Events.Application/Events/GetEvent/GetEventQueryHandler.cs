using Evently.Modules.Events.Domain.Events;
using MediatR;

namespace Evently.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IEventRepository eventRepository) : IRequestHandler<GetEventQuery, EventResponse?>
{

	public async Task<EventResponse?> Handle(GetEventQuery request, CancellationToken cancellationToken)
	{
		var @event = await eventRepository.GetByIdAsync(request.EventId);
		
		if (@event is null) return null;
		
		return new EventResponse(
			@event.Id,
			@event.Title,
			@event.Description,
			@event.Location,
			@event.StartsAtUtc,
			@event.EndsAtUtc);
	}
}