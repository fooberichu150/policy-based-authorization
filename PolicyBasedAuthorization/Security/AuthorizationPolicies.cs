using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Security
{
    public static class AuthorizationPolicies
    {
        public const string DefaultAccess = nameof(DefaultAccess);
        public const string XApiKeyAccess = nameof(XApiKeyAccess);
        public const string XServerKeyAccess = nameof(XServerKeyAccess);
        public const string BasicAuthenticationOrXApiKeyAccess = nameof(BasicAuthenticationOrXApiKeyAccess);

        public static AuthorizationPolicy DefaultAccessPolicy { get; } = CreateDefaultAccessPolicy();
        public static AuthorizationPolicy ServerKeyPolicy { get; } = CreateServerKeyPolicy();
        public static AuthorizationPolicy XApiKeyPolicy { get; } = CreateXApiKeyPolicy();
        public static AuthorizationPolicy BasicAuthenticationOrXApiKeyPolicy { get; } = CreateBasicAuthenticationOrXApiKeyPolicy();

        private static AuthorizationPolicy CreateDefaultAccessPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthenticationSchemes.BasicAuthentication)
                .RequireAuthenticatedUser()
                .Build();
        }

        private static AuthorizationPolicy CreateServerKeyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthenticationSchemes.ApiKey)
                .RequireClaim(ClaimTypes.ApiKeyClaim, HttpHeaderKeys.XServerKey)
                .RequireAuthenticatedUser()
                .Build();
        }

        private static AuthorizationPolicy CreateXApiKeyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthenticationSchemes.ApiKey)
                .RequireClaim(ClaimTypes.ApiKeyClaim, HttpHeaderKeys.XApiKey)
                .RequireAuthenticatedUser()
                .Build();
        }

        private static AuthorizationPolicy CreateBasicAuthenticationOrXApiKeyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthenticationSchemes.BasicAuthentication, AuthenticationSchemes.ApiKey)
                .AddRequirements(new ApiKeyOrPublicResourcesAuthorizationRequirement(HttpHeaderKeys.XApiKey, Roles.Administration))
                .RequireAuthenticatedUser()
                .Build();
        }

        public static void AddAuthorizationPolicies(this AuthorizationOptions options)
		{
            options.AddPolicy(DefaultAccess, DefaultAccessPolicy);
			options.AddPolicy(XServerKeyAccess, ServerKeyPolicy);
			options.AddPolicy(XApiKeyAccess, XApiKeyPolicy);
            options.AddPolicy(BasicAuthenticationOrXApiKeyAccess, BasicAuthenticationOrXApiKeyPolicy);
        }
    }
}
