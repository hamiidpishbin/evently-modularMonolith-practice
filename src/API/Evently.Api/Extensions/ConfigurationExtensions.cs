namespace Evently.Api.Extensions;

internal static class ConfigurationExtensions
{
	internal static void AddModuleConfigurations(this IConfigurationBuilder configurationBuilder, string[] modules)
	{
		foreach (var module in modules)
		{
			configurationBuilder.AddJsonFile($"modules.{module}.json", optional: false, reloadOnChange: true);
			configurationBuilder.AddJsonFile($"modules.{module}.Development.json", optional: true, reloadOnChange: true);
		}
	}
}