using Microsoft.AspNetCore.Mvc;

namespace Weather_WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private IConfiguration _config;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _config = configuration;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        //Reading config value
        string? configValue = _config.GetSection("MyConfigKey").Value;

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            ConfigValue = configValue
        })
        .ToArray();
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<string>> CallThirdParty(string key)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://10.244.0.41:5000/connect/{key}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return Ok($"Success for {key}.");
                }
                else
                {
                    return NotFound();
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest();
        }

    }
}