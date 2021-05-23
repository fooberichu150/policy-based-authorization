using System.Net.Http;

namespace PolicyBasedAuthorization.Integration.Tests.Security
{
    public interface IUserAgent
    {
        public HttpClient Client { get; }
    }
}
