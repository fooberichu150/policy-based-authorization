using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using PolicyBasedAuthorization.Integration.Tests.Constants;
using PolicyBasedAuthorization.Integration.Tests.Controllers;
using PolicyBasedAuthorization.Integration.Tests.Security;
using Xunit;

namespace PolicyBasedAuthorization.Integration.Tests.Scenarios
{
	public class AuthorizationRequiredTests
	{
		private WeatherForecastTestController _controller;

		public AuthorizationRequiredTests()
		{
			_controller = new WeatherForecastTestController();
		}

		[Fact]
		public async Task Get_WeatherForecasts_AllowsAnonymous_Async()
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(AuthenticationType.Anonymous);

			var response = await _controller.GET_WeatherForecasts(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.BasicAuthentication, true)]
		[InlineData(AuthenticationType.BasicAuthentication, false)]
		[InlineData(AuthenticationType.XApiKey, false)]
		[InlineData(AuthenticationType.ServerKey, false)]
		public async Task Get_WeatherForecasts_Allows_Authenticated_Async(AuthenticationType authenticationType, bool isAdmin)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType, isAdmin);

			var response = await _controller.GET_WeatherForecasts(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.InvalidXApiKey)]
		[InlineData(AuthenticationType.InvalidServerKey)]
		public async Task Get_WeatherForecasts_Allows_InvalidApiKey_Async(AuthenticationType authenticationType)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType);

			var response = await _controller.GET_WeatherForecasts(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.BasicAuthentication, true)]
		[InlineData(AuthenticationType.BasicAuthentication, false)]
		public async Task Get_Sample01_Allows_Authenticated_Async(AuthenticationType authenticationType, bool isAdmin)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType, isAdmin);

			var response = await _controller.GET_Sample01(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.XApiKey)]
		[InlineData(AuthenticationType.ServerKey)]
		public async Task Get_Sample01_Rejects_ApiKey_Async(AuthenticationType authenticationType)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType);

			var response = await _controller.GET_Sample01(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
		}

		[Fact]
		public async Task Get_Sample01_Rejects_Anonymous_Async()
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(AuthenticationType.Anonymous);

			var response = await _controller.GET_Sample01(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
		}

		[Theory]
		[InlineData(AuthenticationType.BasicAuthentication, true)]
		[InlineData(AuthenticationType.BasicAuthentication, false)]
		public async Task Get_Sample02_Allows_Authenticated_Async(AuthenticationType authenticationType, bool isAdmin)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType, isAdmin);

			var response = await _controller.GET_Sample02(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.XApiKey)]
		[InlineData(AuthenticationType.ServerKey)]
		public async Task Get_Sample02_Rejects_ApiKey_Async(AuthenticationType authenticationType)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType);

			var response = await _controller.GET_Sample02(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
		}

		[Fact]
		public async Task Get_Sample02_Rejects_Anonymous_Async()
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(AuthenticationType.Anonymous);

			var response = await _controller.GET_Sample02(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
		}

		[Theory]
		[InlineData(TestAuthorization.ApiKey)]
		[InlineData(TestAuthorization.ApiAlternateKey)]
		public async Task Get_Sample03_Accepts_XApiKey_Async(string apiKey)
		{
			var authenticatedUser = UserAgentFactory.CreateApiKeyAgent(AuthenticationType.XApiKey, apiKey);

			var response = await _controller.GET_Sample03(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.Anonymous, false)]
		[InlineData(AuthenticationType.BasicAuthentication, true)]
		[InlineData(AuthenticationType.BasicAuthentication, false)]
		[InlineData(AuthenticationType.InvalidServerKey, false)]
		[InlineData(AuthenticationType.InvalidXApiKey, false)]
		public async Task Get_Sample03_Rejects_Non_XApiKey_Async(AuthenticationType authenticationType, bool isAdmin)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType, isAdmin);

			var response = await _controller.GET_Sample03(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
		}

		[Fact]
		public async Task Get_Sample03_Forbids_XServerKey_Async()
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(AuthenticationType.ServerKey);

			var response = await _controller.GET_Sample03(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
		}

		[Theory]
		[InlineData(TestAuthorization.ApiKey)]
		[InlineData(TestAuthorization.ApiAlternateKey)]
		public async Task Get_Sample04_Accepts_XServerKey_Async(string apiKey)
		{
			var authenticatedUser = UserAgentFactory.CreateApiKeyAgent(AuthenticationType.ServerKey, apiKey);

			var response = await _controller.GET_Sample04(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.Anonymous, false)]
		[InlineData(AuthenticationType.BasicAuthentication, true)]
		[InlineData(AuthenticationType.BasicAuthentication, false)]
		[InlineData(AuthenticationType.InvalidServerKey, false)]
		[InlineData(AuthenticationType.InvalidXApiKey, false)]
		public async Task Get_Sample04_Rejects_Non_XServerKey_Async(AuthenticationType authenticationType, bool isAdmin)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType, isAdmin);

			var response = await _controller.GET_Sample04(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
		}

		[Fact]
		public async Task Get_Sample04_Forbids_XApiKey_Async()
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(AuthenticationType.XApiKey);

			var response = await _controller.GET_Sample04(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
		}

		[Theory]
		[InlineData(AuthenticationType.BasicAuthentication, true)]
		[InlineData(AuthenticationType.XApiKey, false)]
		public async Task Get_Sample05_Accepts_Valid_Authentication_Async(AuthenticationType authenticationType, bool isAdmin)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType, isAdmin);

			var response = await _controller.GET_Sample05(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status200OK);
		}

		[Theory]
		[InlineData(AuthenticationType.ServerKey, false)]
		[InlineData(AuthenticationType.BasicAuthentication, false)]
		public async Task Get_Sample05_Rejects_Invalid_Authentication_Async(AuthenticationType authenticationType, bool isAdmin)
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(authenticationType, isAdmin);

			var response = await _controller.GET_Sample05(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
		}

		[Fact]
		public async Task Get_Sample05_Rejects_Anonymous_Async()
		{
			var authenticatedUser = UserAgentFactory.CreateUserAgent(AuthenticationType.Anonymous);

			var response = await _controller.GET_Sample05(authenticatedUser);
			response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
		}
	}
}
