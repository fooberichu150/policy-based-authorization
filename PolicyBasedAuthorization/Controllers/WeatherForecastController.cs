using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PolicyBasedAuthorization.Models;
using PolicyBasedAuthorization.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace PolicyBasedAuthorization.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, Services.IUserService userService)
		{
			_logger = logger;
		}

		[AllowAnonymous]
		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}


		/// <summary>
		/// This endpoint only allows logged in users (since we use a FallbackPolicy)
		/// </summary>
		/// <returns></returns>
		[HttpGet("sample01")]
		[SwaggerOperation(Summary = "This endpoint only allows logged in users (since we use a FallbackPolicy)")]
		public IActionResult Get_Sample01()
		{
			return Ok();
		}

		/// <summary>
		/// This endpoint allows authenticated Basic Authentication users
		/// </summary>
		/// <returns></returns>
		[HttpGet("sample02")]
		[Authorize(Policy = AuthorizationPolicies.DefaultAccess)]
		[SwaggerOperation(Summary = "This endpoint explicitly only allows authenticated Basic Authentication users")]
		public IActionResult Get_Sample02()
		{
			return Ok();
		}

		/// <summary>
		/// This endpoint only allows X-API-KEY access
		/// </summary>
		/// <returns></returns>
		[HttpGet("sample03")]
		[Authorize(Policy = AuthorizationPolicies.XApiKeyAccess)]
		[SwaggerOperation(Summary = "This endpoint only allows X-API-KEY access")]
		public IActionResult Get_Sample03()
		{
			return Ok();
		}

		/// <summary>
		/// This endpoint only allows X-SERVER-KEY access
		/// </summary>
		/// <returns></returns>
		[HttpGet("sample04")]
		[Authorize(Policy = AuthorizationPolicies.XServerKeyAccess)]
		[SwaggerOperation(Summary = "This endpoint only allows X-SERVER-KEY access")]
		public IActionResult Get_Sample04()
		{
			return Ok();
		}

		/// <summary>
		/// This endpoint only allows Admin users or X-API-KEY access
		/// </summary>
		/// <returns></returns>
		[HttpGet("sample05")]
		[Authorize(Policy = AuthorizationPolicies.BasicAuthenticationOrXApiKeyAccess)]
		[SwaggerOperation(Summary = "This endpoint only allows Admin users or X-API-KEY access")]
		public IActionResult Get_Sample05()
		{
			return Ok();
		}
	}
}
