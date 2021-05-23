using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using PolicyBasedAuthorization.Integration.Tests.Constants;

namespace PolicyBasedAuthorization.Integration.Tests.TestServer
{
    public class TestPolicyBasedAuthorizationApiFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
            .UseEnvironment("Development")
            .ConfigureAppConfiguration(builder =>
             {
                 //This will override configuration values
                 builder.AddInMemoryCollection(new Dictionary<string, string>
                 {
                     ["App:ApiKey"] = TestAuthorization.ValidApiKeys,
                     ["App:ServerApiKey"] = TestAuthorization.ValidApiKeys
                 });
             })
            .ConfigureTestServices(services =>
            {
            })
            .ConfigureServices(services =>
            {
            });
        }
    }
}