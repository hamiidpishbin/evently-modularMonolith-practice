using Evently.Api.Extensions;
using Evently.Modules.Events.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.Run();