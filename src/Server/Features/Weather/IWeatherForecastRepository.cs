using Shared.Features.Weather;

namespace Server.Features.Weather;

public interface IWeatherForecastRepository
{
    Task<IEnumerable<WeatherForecast>> GetForecastAsync(int days, CancellationToken cancellationToken = default);
}
