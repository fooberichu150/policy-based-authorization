using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace PolicyBasedAuthorization.Security
{
	public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
	{
		/// <summary>
		/// Initializes a new instance of <see cref="ApiKeyAuthenticationHandler`1"/>.
		/// </summary>
		/// <param name="options">The monitor for the options instance.</param>
		/// <param name="logger">The <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/>.</param>
		/// <param name="encoder">The <see cref="System.Text.Encodings.Web.UrlEncoder"/>.</param>
		/// <param name="clock">The <see cref="Microsoft.AspNetCore.Authentication.ISystemClock"/>.</param>
		public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
		{

		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
            var hasXApiKey = Request.Headers.TryGetValue(HttpHeaderKeys.XApiKey, out var apiKey);
            var hasServerKey = Request.Headers.TryGetValue(HttpHeaderKeys.XServerKey, out var globalApiKey);

            if (!hasXApiKey && !hasServerKey)
            {
                return Task.FromResult(AuthenticateResult.Fail("No authorization header detected."));
            }

            if (StringValues.IsNullOrEmpty(apiKey)
                && StringValues.IsNullOrEmpty(globalApiKey))
            {
                return Task.FromResult(AuthenticateResult.Fail("Cannot read authorization header."));
            }

            var ticket = GetAuthenticationTicket(Options.ApiKey, apiKey, HttpHeaderKeys.XApiKey)
                ?? GetAuthenticationTicket(Options.ServerApiKey, globalApiKey, HttpHeaderKeys.XServerKey);

            if (ticket == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid auth key."));
            }

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private AuthenticationTicket? GetAuthenticationTicket(string? acceptedKeys, string incomingKey, string claimValue)
        {
            var validKeys = SplitKey(acceptedKeys)
                .Distinct()
                .ToArray();

            // use-case to handle IIS vs Kestrel header management weirdness
            var incomingKeys = SplitKey(incomingKey)
                .Distinct()
                .ToArray();

            if (!validKeys.Any(key => incomingKeys.Any(incomingKey => string.Compare(key, incomingKey) == 0)))
            {
                return null;
            }

            var serverKeyIdentity = new ClaimsIdentity(AuthenticationSchemes.ApiKey);
            serverKeyIdentity.AddClaim(new Claim(ClaimTypes.ApiKeyClaim, claimValue));

            return new AuthenticationTicket(new ClaimsPrincipal(serverKeyIdentity), AuthenticationSchemes.ApiKey);
        }

        private IEnumerable<string> SplitKey(string? key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return Array.Empty<string>();

            return key.Split(',').Select(innerKey => innerKey.Trim()).ToArray();
        }
    }
}
