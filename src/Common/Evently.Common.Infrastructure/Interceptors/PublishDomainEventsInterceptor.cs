using Evently.Common.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Outbox;

public sealed class PublishDomainEventsInterceptor(IServiceScopeFactory serviceScopeFactory) : SaveChangesInterceptor
{
	public override async ValueTask<int> SavedChangesAsync(
		SaveChangesCompletedEventData eventData,
		int result,
		CancellationToken cancellationToken = new CancellationToken())
	{
		if (eventData.Context is not null)
		{
			await PublishDomainEventsAsync(eventData.Context);
		}

		return await base.SavedChangesAsync(eventData, result, cancellationToken);
	}
	
	private async Task PublishDomainEventsAsync(DbContext context)
	{
		var domainEvents = context
			.ChangeTracker
			.Entries<Entity>()
			.Select(entry => entry.Entity)
			.SelectMany(domainEntity =>
			{
				var domainEvents = domainEntity.DomainEvents;

				domainEntity.ClearDomainEvents();

				return domainEvents;
			})
			.ToList();

		using var scope = serviceScopeFactory.CreateScope();

		var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

		foreach (var domainEvent in domainEvents)
		{
			await publisher.Publish(domainEvent);
		}
	}
}