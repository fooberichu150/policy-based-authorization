using System.Net.Http;
using PolicyBasedAuthorization.Integration.Tests.Constants;
using PolicyBasedAuthorization.Integration.Tests.TestServer;

namespace PolicyBasedAuthorization.Integration.Tests.Security
{
	public class ApiKeyUserAgent : IUserAgent
	{
        public ApiKeyUserAgent(TestPolicyBasedAuthorizationApiFactory apiFactory,
            string headerKey,
            string apiKey = null)
        {
            Client = apiFactory.CreateClient();
            Client.DefaultRequestHeaders.Add(headerKey, apiKey ?? TestAuthorization.ApiKey);
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
