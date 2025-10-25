using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Features.Weather;

namespace Server.Features.Weather;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeatherController : ControllerBase
{
    private readonly IWeatherForecastRepository _repository;

    public WeatherController(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get(CancellationToken cancellationToken)
    {
        var forecasts = await _repository.GetForecastAsync(5, cancellationToken);
        return Ok(forecasts);
    }
}
