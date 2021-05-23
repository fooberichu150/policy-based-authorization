namespace PolicyBasedAuthorization.Security
{
	public static class HttpHeaderKeys
	{
		public const string XApiKey = "X-Api-Key";
		public const string XServerKey = "X-Server-Key";
	}

	public static class ClaimTypes
	{
		public const string ApiKeyClaim = "https://apis.seeleycoder.com/claims/api-key-type";
	}

	public static class AuthenticationSchemes
	{
        public const string ApiKey = "ApiKey";
		public const string BasicAuthentication = "Basic";
	}
}
