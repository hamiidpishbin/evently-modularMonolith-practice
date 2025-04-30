namespace Evently.Common.Application.EventBus;

public interface IIntegrationEvent
{
	public Guid Id { get; }
	public DateTime OccurredOnUtc { get; }
}

public abstract class IntegrationEvent(Guid id, DateTime occurredOnUtc) : IIntegrationEvent
{
	public Guid Id { get; init; } = id;
	public DateTime OccurredOnUtc { get; init; } = occurredOnUtc;
}