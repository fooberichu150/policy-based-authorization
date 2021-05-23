using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolicyBasedAuthorization.Models;
using PolicyBasedAuthorization.Security;

namespace PolicyBasedAuthorization.Services
{
	public interface IUserService
	{
		BasicUser AuthenticateUser(string userName, string password);
	}

	public class UserService : IUserService
	{
		private List<BasicUser> _users = new List<BasicUser>();

		public UserService(IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			System.Text.Encodings.Web.UrlEncoder encoder,
			Microsoft.AspNetCore.Authentication.ISystemClock clock)
		{
			_users.Add(new BasicUser { UserId = 1, UserName = "admin", Password = "password123", Roles = new[] { Roles.Administration } });
			_users.Add(new BasicUser { UserId = 2, UserName = "standard", Password = "password123" });
		}

		public BasicUser AuthenticateUser(string userName, string password)
		{
			var user = _users.FirstOrDefault(u => string.Compare(u.UserName, userName, StringComparison.OrdinalIgnoreCase) == 0);

			return user?.Password == password ? user : null;
		}
	}
}
