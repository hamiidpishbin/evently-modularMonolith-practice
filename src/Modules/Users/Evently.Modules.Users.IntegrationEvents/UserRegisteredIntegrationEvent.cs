using Evently.Common.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public class UserRegisteredIntegrationEvent(
	Guid id, 
	DateTime occurredOnUtc,
	Guid userId,
	string email,
	string firstName,
	string lastName) : IntegrationEvent(id, occurredOnUtc)
{
	public Guid UserId { get; set; } = userId;
	public string Email { get; set; } = email;
	public string FirstName { get; set; } = firstName;
	public string LastName { get; set; } = lastName;
}