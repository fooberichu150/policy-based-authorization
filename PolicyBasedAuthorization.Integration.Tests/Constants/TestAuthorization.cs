using PolicyBasedAuthorization.Models;

namespace PolicyBasedAuthorization.Integration.Tests.Constants
{
    public static class TestAuthorization
    {
        public const int WellKnownTestAdminId = 1;
        public const int WellKnownTestUserId = 2;

        public const string ApiKey = "TestApiKey";
        public const string ApiAlternateKey = "AnotherTestApiKey";
        public const string ValidApiKeys = "TestApiKey,AnotherTestApiKey";
        public const string BadApiKey = "BadApiKey";

        public enum AuthorizationType
        {
            ApiKey,
            Token
        }

        public static BasicUser AdminUser = new BasicUser { UserName = "admin", Password = "password123" };
        public static BasicUser StandardUser = new BasicUser { UserName = "standard", Password = "password123" };
    }
}