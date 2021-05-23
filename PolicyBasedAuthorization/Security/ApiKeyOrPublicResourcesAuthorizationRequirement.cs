using System;
using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Security
{
	// intentionally left blank; we can add to this if we need to customize requirements per policy but right now the policy only cares that
	// the requirement is met at all
	// see: https://github.com/blowdart/AspNetAuthorizationWorkshop#step-5-code-based-policies for more information
	public class ApiKeyOrPublicResourcesAuthorizationRequirement : IAuthorizationRequirement
	{
		public ApiKeyOrPublicResourcesAuthorizationRequirement(string? apiKeyType, params string[] acceptRoles)
		{
			RequireApiKeyType = apiKeyType;
			Roles = acceptRoles ?? Array.Empty<string>();
		}

		public ApiKeyOrPublicResourcesAuthorizationRequirement(params string[] acceptRoles)
			: this(null, acceptRoles)
		{
		}

		public string? RequireApiKeyType { get; }

		public string[] Roles { get; }
	}
}
