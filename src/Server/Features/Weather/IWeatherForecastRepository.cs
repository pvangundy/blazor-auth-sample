using BlazorAuthSample.Shared;

namespace BlazorAuthSample.Server.Features.Weather;

public interface IWeatherForecastRepository
{
    Task<IReadOnlyList<WeatherForecastDto>> GetAsync(CancellationToken cancellationToken);
}
