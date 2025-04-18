using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Presentation.Events;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
	public static void MapEndpoints(IEndpointRouteBuilder app)
	{
		EventEndpoints.MapEndpoints(app);
	}

	public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
		});
		
		services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);
		
		services.AddInfrastructure(configuration);
		
		return services;
	}

	private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<EventsDbContext>(options =>
		{
			options.UseNpgsql(
				configuration.GetConnectionString("Database"),
				npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events));
		});
		
		services.AddScoped<IEventRepository, EventRepository>();
		
		services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
	}
}