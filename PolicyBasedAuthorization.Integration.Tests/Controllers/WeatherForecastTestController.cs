using System.Net.Http;
using System.Threading.Tasks;
using PolicyBasedAuthorization.Integration.Tests.Security;

namespace PolicyBasedAuthorization.Integration.Tests.Controllers
{
	public class WeatherForecastTestController
	{
		public Task<HttpResponseMessage> GET_WeatherForecasts(IUserAgent agent, string extraPath = null)
		{
			string uri = "weatherforecast";
			if (!string.IsNullOrWhiteSpace(extraPath))
				uri = string.Concat(uri, "/", extraPath);

			var request = new HttpRequestMessage(HttpMethod.Get, uri);

			return agent.Client.SendAsync(request);
		}

		public Task<HttpResponseMessage> GET_Sample01(IUserAgent agent) 
			=> GET_WeatherForecasts(agent, "sample01");

		public Task<HttpResponseMessage> GET_Sample02(IUserAgent agent)
			=> GET_WeatherForecasts(agent, "sample02");

		public Task<HttpResponseMessage> GET_Sample03(IUserAgent agent)
			=> GET_WeatherForecasts(agent, "sample03");

		public Task<HttpResponseMessage> GET_Sample04(IUserAgent agent)
			=> GET_WeatherForecasts(agent, "sample04");

		public Task<HttpResponseMessage> GET_Sample05(IUserAgent agent)
			=> GET_WeatherForecasts(agent, "sample05");
	}
}
