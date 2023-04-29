using Business.TokenHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
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

		private readonly IUserTokenIssuer _issuer;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserTokenIssuer issuer)
		{
			_logger = logger;
			_issuer = issuer;
		}

		[HttpGet]
		public string Get()
		{
			return "";
		}
	}
}
