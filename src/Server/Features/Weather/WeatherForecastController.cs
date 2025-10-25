using BlazorAuthSample.Shared.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuthSample.Server.Features.Weather;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastRepository _repository;

    public WeatherForecastController(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WeatherForecastDto>>> Get(CancellationToken cancellationToken)
    {
        var forecasts = await _repository.GetAsync(cancellationToken);
        return Ok(forecasts);
    }
}
