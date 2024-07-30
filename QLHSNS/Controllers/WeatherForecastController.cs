using Microsoft.AspNetCore.Mvc;
using QLHSNS.Common.Interfaces;
using QLHSNS.DTOs.SendEmail;

namespace QLHSNS.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase {
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		private readonly IEmailService _emailService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailService emailService) {
			_logger = logger;
			_emailService = emailService;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get() {
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpPost("TestSendEmail")]
		public async Task<IActionResult> TestSendEmail([FromBody] SendEmailRequest sendEmailRequest) {
			await _emailService.SendEmailAsync(sendEmailRequest);
			return Ok("Email sent succesfully");
		}
	}
}
