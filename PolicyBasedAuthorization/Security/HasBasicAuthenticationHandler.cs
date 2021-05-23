using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Security
{
	public class HasBasicAuthenticationHandler : AuthorizationHandler<ApiKeyOrPublicResourcesAuthorizationRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyOrPublicResourcesAuthorizationRequirement requirement)
		{
			if (context.User.Identity.AuthenticationType != "BasicAuthentication")
			{
				return Task.CompletedTask;
			}

			var hasRoles = new Func<bool>(() => requirement.Roles.Any(role => context.User.IsInRole(role)));
			if (requirement.Roles.Length == 0 || hasRoles())
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
