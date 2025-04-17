using Evently.Modules.Events.Api.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Api.Events;

public static class GetEvent
{
	public static void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("events/{id}",
			async (Guid id, EventsDbContext context) =>
			{
				var result = await context.Events
					.Where(e => e.Id == id)
					.Select(x => new EventResponse(x.Id,
						x.Title,
						x.Description,
						x.Location,
						x.StartsAtUtc,
						x.EndsAtUtc))
					.SingleOrDefaultAsync();
				
				return result is null ? Results.NotFound() : Results.Ok(result);
			}).WithTags(Tags.Events);
	}
}