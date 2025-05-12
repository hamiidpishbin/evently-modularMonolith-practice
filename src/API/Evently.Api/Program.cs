using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Configuration.AddModuleConfigurations(["events", "users", "ticketing"]);

builder.Services.AddApplication([
	Evently.Modules.Events.Application.AssemblyReference.Assembly,
	Evently.Modules.Users.Application.AssemblyReference.Assembly,
	Evently.Modules.Ticketing.Application.AssemblyReference.Assembly
]);

var databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

builder.Services.AddInfrastructure(
	[TicketingModule.ConfigureConsumers],
	databaseConnectionString, 
	redisConnectionString);

builder.Services.AddHealthChecks()
	.AddNpgSql(databaseConnectionString)
	.AddRedis(redisConnectionString)
	.AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("Keycloak:HealthUrl")!), HttpMethod.Get, "keycloak");

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	app.ApplyMigrations();
}

app.MapGet("/", () => "Hello World!");

app.MapHealthChecks("health",
	new HealthCheckOptions()
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.MapEndpoints();

app.UseAuthentication();

app.UseAuthorization();

app.Run();