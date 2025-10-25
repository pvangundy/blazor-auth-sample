using BlazorAuthSample.Shared;

namespace BlazorAuthSample.Client.Features.Weather.Services;

public class WeatherForecastService
{
    private readonly WeatherForecastApiClient _apiClient;

    public WeatherForecastService(WeatherForecastApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyList<WeatherForecastDto>> GetForecastAsync(CancellationToken cancellationToken = default) =>
        _apiClient.GetForecastAsync(cancellationToken);
}
