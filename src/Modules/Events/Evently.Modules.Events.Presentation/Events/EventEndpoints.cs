using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public static class EventEndpoints
{
	public static void MapEndpoints(this IEndpointRouteBuilder app)
	{
		CreateEvent.MapEndpoint(app);
		GetEvent.MapEndpoint(app);
	}
}