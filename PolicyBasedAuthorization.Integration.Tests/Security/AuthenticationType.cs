namespace PolicyBasedAuthorization.Integration.Tests.Security
{
	public enum AuthenticationType
    {
        Anonymous,
        BasicAuthentication,
        ServerKey,
        InvalidServerKey,
        XApiKey,
        InvalidXApiKey
    }
}
