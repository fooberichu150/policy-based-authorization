using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace PolicyBasedAuthorization.Security
{
	public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
	{
		private string? _apiKey;

		/// <summary>
		/// Accepted key(s) for X-Api-Key for external requests
		/// </summary>
		public string? ApiKey
		{
			get => _apiKey;
			set
			{
				_apiKey = value;
				ApiKeys = !string.IsNullOrWhiteSpace(value)
					? value.Split(",").ToList()
					: new List<string>();
			}
		}

		/// <summary>
		/// Used to accept X-Server-Key from internal requests.
		/// </summary>
		public string? ServerApiKey { get; set; }

		public IReadOnlyList<string>? ApiKeys { get; private set; }
	}
}
