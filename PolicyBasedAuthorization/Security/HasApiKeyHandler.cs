using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Security
{
	public class HasApiKeyHandler : AuthorizationHandler<ApiKeyOrPublicResourcesAuthorizationRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyOrPublicResourcesAuthorizationRequirement requirement)
		{
			if (context.User.Identity.AuthenticationType != AuthenticationSchemes.ApiKey)
			{
				return Task.CompletedTask;
			}

			if (!string.IsNullOrWhiteSpace(requirement.RequireApiKeyType)
				&& !context.User.HasClaim(c => c.Type == ClaimTypes.ApiKeyClaim && c.Value == requirement.RequireApiKeyType))
			{
				return Task.CompletedTask;
			}

			context.Succeed(requirement);
			return Task.CompletedTask;
		}
	}
}
