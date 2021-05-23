using System;
using PolicyBasedAuthorization.Integration.Tests.Constants;
using PolicyBasedAuthorization.Integration.Tests.TestServer;
using PolicyBasedAuthorization.Security;

namespace PolicyBasedAuthorization.Integration.Tests.Security
{
	public static class UserAgentFactory
    {
        private const bool AdminRequested = true;
        private const bool AdminNotRequested = false;

        public static IUserAgent CreateUserAgent(AuthenticationType type)
            => CreateUserAgent(type, false);

        public static IUserAgent CreateApiKeyAgent(AuthenticationType type, string apiKey = null)
        {
            return (type) switch
            {
				AuthenticationType.ServerKey => new ApiKeyUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, HttpHeaderKeys.XServerKey, apiKey),
				AuthenticationType.XApiKey => new ApiKeyUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, HttpHeaderKeys.XApiKey, apiKey),
                _ => throw new NotImplementedException()
            };
        }

        public static IUserAgent CreateUserAgent(AuthenticationType type, bool isAdmin = false)
        {
            return (type, isAdmin) switch
            {
                (AuthenticationType.BasicAuthentication, AdminNotRequested) => new BasicAuthenticationUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, TestAuthorization.StandardUser.UserName, TestAuthorization.StandardUser.Password),
                (AuthenticationType.BasicAuthentication, AdminRequested) => new BasicAuthenticationUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, TestAuthorization.AdminUser.UserName, TestAuthorization.AdminUser.Password),
                (AuthenticationType.ServerKey, _) => new ApiKeyUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, HttpHeaderKeys.XServerKey),
                (AuthenticationType.InvalidServerKey, _) => new ApiKeyUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, HttpHeaderKeys.XServerKey, TestAuthorization.BadApiKey),
                (AuthenticationType.XApiKey, _) => new ApiKeyUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, HttpHeaderKeys.XApiKey),
                (AuthenticationType.InvalidXApiKey, _) => new ApiKeyUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api, HttpHeaderKeys.XApiKey, TestAuthorization.BadApiKey),
                (AuthenticationType.Anonymous, _) => new AnonymousUserAgent(TestPolicyBasedAuthorizationApi.Instance.Api),
                _ => throw new NotImplementedException()
            };
        }
    }
}
