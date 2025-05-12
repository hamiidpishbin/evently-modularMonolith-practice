using System.Net.Http.Json;

namespace Evently.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{
	internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
	{
		var httpResponseMessage = await httpClient.PostAsJsonAsync("users", user, cancellationToken);

		httpResponseMessage.EnsureSuccessStatusCode();

		return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
	}
	
	private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
	{
		const string userSegmentName = "users/";

		var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

		if (locationHeader is null)
		{
			throw new InvalidOperationException("Location header is null");
		}

		var userValueSegmentIndex = locationHeader.IndexOf(userSegmentName, StringComparison.InvariantCultureIgnoreCase);

		return locationHeader.Substring(userValueSegmentIndex + userSegmentName.Length);
	}
}