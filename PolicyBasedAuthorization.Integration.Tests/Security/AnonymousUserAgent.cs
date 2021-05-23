using System.Net.Http;
using PolicyBasedAuthorization.Integration.Tests.TestServer;

namespace PolicyBasedAuthorization.Integration.Tests.Security
{
	public class AnonymousUserAgent : IUserAgent
	{
        public AnonymousUserAgent(TestPolicyBasedAuthorizationApiFactory apiFactory)
        {
            Client = apiFactory.CreateClient();
        }

        public HttpClient Client { get; }
    }
}
