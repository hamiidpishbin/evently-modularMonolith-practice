using Evently.Common.Application.Caching;
using Evently.Common.Application.Clock;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Caching;
using Evently.Common.Infrastructure.Clock;
using Evently.Common.Infrastructure.Data;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		Action<IRegistrationConfigurator>[] moduleConfigureConsumers,
		string databaseConnectionString,
		string redisConnectionString)
	{
		var npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
		services.TryAddSingleton(npgsqlDataSource);

		services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

		services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

		try
		{
			IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

			services.TryAddSingleton(connectionMultiplexer);

			services.AddStackExchangeRedisCache(options =>
			{
				options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
			});
		}

		catch
		{
			services.AddDistributedMemoryCache();
		}

		services.TryAddSingleton<ICacheService, CacheService>();
		
		services.TryAddSingleton<IEventBus, EventBus.EventBus>();

		services.AddMassTransit(configure =>
		{
			foreach (var moduleConfigureConsumer in moduleConfigureConsumers)
			{
				moduleConfigureConsumer(configure);
			}
			
			configure.SetKebabCaseEndpointNameFormatter();
			
			configure.UsingInMemory((context, cfg) =>
			{
				cfg.ConfigureEndpoints(context);
			});
		});

		return services;
	}
}