using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Modules.Events.Infrastructure;
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

builder.Configuration.AddModuleConfigurations(["events"]);

builder.Services.AddApplication([Evently.Modules.Events.Application.AssemblyReference.Assembly]);

var databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

builder.Services.AddInfrastructure(databaseConnectionString, redisConnectionString);

builder.Services.AddHealthChecks()
	.AddNpgSql(databaseConnectionString)
	.AddRedis(redisConnectionString);

builder.Services.AddEventsModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	app.ApplyMigrations();
}

app.MapGet("/", () => "Hello World!");

EventsModule.MapEndpoints(app);

app.MapHealthChecks("health",
	new HealthCheckOptions()
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.Run();