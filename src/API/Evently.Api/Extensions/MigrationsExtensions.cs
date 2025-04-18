using Evently.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Api.Extensions;

internal static class MigrationsExtensions
{
	internal static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		
		ApplyMigration<EventsDbContext>(scope);
	}
	
	private static void ApplyMigration<TDbContext>(IServiceScope scope) 
		where TDbContext : DbContext
	{
		using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
		
		context.Database.Migrate();
	}
}