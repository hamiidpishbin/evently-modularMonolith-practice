﻿using Evently.Common.Domain;
using Evently.Common.Presentation.Endpoints;
using Evently.Common.Presentation.Results;
using Evently.Modules.Events.Application.Events.GetEvents;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal class GetEvents : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("events",
				async (ISender sender) =>
				{
					var result = await sender.Send(new GetEventsQuery());

					return result.Match(Results.Ok, ApiResults.Problem);
				})
			.WithTags(Tags.Events);
	}
}