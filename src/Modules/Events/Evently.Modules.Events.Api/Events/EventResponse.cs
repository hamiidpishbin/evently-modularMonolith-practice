namespace Evently.Modules.Events.Api.Events;

internal record EventResponse(Guid Id,
	string Title,
	string Description,
	string Location,
	DateTime StartsAtUtc,
	DateTime EndsAtUtc);