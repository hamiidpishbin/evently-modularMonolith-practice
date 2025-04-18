namespace Evently.Modules.Events.Application.Events.GetEvent;

internal record EventResponse(Guid Id,
	string Title,
	string Description,
	string Location,
	DateTime StartsAtUtc,
	DateTime? EndsAtUtc);