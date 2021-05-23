namespace PolicyBasedAuthorization.Integration.Tests.TestServer
{
	public sealed class TestPolicyBasedAuthorizationApi
    {
        public TestPolicyBasedAuthorizationApiFactory Api { get; }

        public static TestPolicyBasedAuthorizationApi Instance { get; } = new TestPolicyBasedAuthorizationApi();

        static TestPolicyBasedAuthorizationApi() { }
        private TestPolicyBasedAuthorizationApi()
        {
            Api = new TestPolicyBasedAuthorizationApiFactory();
        }
    }
}
