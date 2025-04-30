using Evently.Common.Application.Exceptions;
using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public class UserRegisteredIntegrationEventConsumer(ISender sender) : IConsumer<UserRegisteredIntegrationEvent>
{

	public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
	{
		var createCustomerCommand = new CreateCustomerCommand(
			context.Message.UserId, 
			context.Message.Email, 
			context.Message.FirstName,
			context.Message.LastName);

		var result = await sender.Send(createCustomerCommand);

		if (result.IsFailure)
		{
			throw new EventlyException(nameof(createCustomerCommand), result.Error);
		}
	}
}