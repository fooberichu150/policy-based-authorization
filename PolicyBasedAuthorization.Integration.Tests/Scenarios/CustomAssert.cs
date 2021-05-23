namespace PolicyBasedAuthorization.Integration.Tests.Scenarios
{
	public static class CustomAssert
	{
		public static void Fail(string message = null)
			=> throw new Xunit.Sdk.XunitException(message);
	}
}
