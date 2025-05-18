using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Extensions;

internal static class MigrationsExtensions
{
	internal static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		
		ApplyMigration<UsersDbContext>(scope);
		ApplyMigration<EventsDbContext>(scope);
		ApplyMigration<TicketingDbContext>(scope);
	}
	
	private static void ApplyMigration<TDbContext>(IServiceScope scope) 
		where TDbContext : DbContext
	{
		using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
		
		context.Database.Migrate();
	}
}