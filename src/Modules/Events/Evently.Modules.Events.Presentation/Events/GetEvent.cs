using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.Events.GetEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal class GetEvent : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("events/{id}",
				async (Guid id, ISender sender) =>
				{
					var result = await sender.Send(new GetEventQuery(id));

					return result is null
						? Results.NotFound()
						: Results.Ok(result);
				})
			.WithTags(Tags.Events);
	}
}