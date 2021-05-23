using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolicyBasedAuthorization.Extensions;
using PolicyBasedAuthorization.Security.Models;
using PolicyBasedAuthorization.Services;

namespace PolicyBasedAuthorization.Security
{
	public class BasicAuthenticationSchemeOptions: AuthenticationSchemeOptions
	{

	}

	/// <summary>
	/// Gracefully lifted from: https://www.roundthecode.com/dotnet/how-to-add-basic-authentication-to-asp-net-core-application
	/// Modified for this blog post
	/// </summary>
	public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationSchemeOptions>
	{
		private readonly IUserService _userService;

		/// <summary>
		/// Initializes a new instance of <see cref="BasicAuthenticationHandler`1"/>.
		/// </summary>
		/// <param name="options">The monitor for the options instance.</param>
		/// <param name="logger">The <see cref="Microsoft.Extensions.Logging.ILoggerFactory"/>.</param>
		/// <param name="encoder">The <see cref="System.Text.Encodings.Web.UrlEncoder"/>.</param>
		/// <param name="clock">The <see cref="Microsoft.AspNetCore.Authentication.ISystemClock"/>.</param>
		public BasicAuthenticationHandler(
			IUserService userService,
			IOptionsMonitor<BasicAuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock
			) : base(options, logger, encoder, clock)
		{
			_userService = userService;
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			Response.Headers.Add("WWW-Authenticate", "Basic");

			if (!Request.Headers.ContainsKey("Authorization"))
			{
				return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
			}

			// Get authorization key
			var authorizationHeader = Request.Headers["Authorization"].ToString();
			var authHeaderRegex = new Regex(@"Basic (.*)");

			if (!authHeaderRegex.IsMatch(authorizationHeader))
			{
				return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
			}

			var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
			var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
			var authUsername = authSplit[0];
			var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

			var basicUser = _userService.AuthenticateUser(authUsername, authPassword);
			if (basicUser == null)
			{
				return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
			}

			var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, basicUser.UserName);
			var identity = new ClaimsIdentity(authenticatedUser);

			if (!basicUser.Roles.IsNullOrEmpty())
			{
				identity.AddClaims(basicUser.Roles.Select(r => new Claim(identity.RoleClaimType, r)));
			}

			var claimsPrincipal = new ClaimsPrincipal(identity);

			return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
		}
	}
}
