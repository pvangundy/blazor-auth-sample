using System.Collections.Generic;
using System.Net.Http.Json;
using BlazorAuthSample.Shared.Weather;

namespace BlazorAuthSample.Client.Features.Weather;

public interface IWeatherApiClient
{
    Task<IReadOnlyList<WeatherForecastDto>> GetForecastAsync(CancellationToken cancellationToken = default);
}

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<WeatherForecastDto>> GetForecastAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<List<WeatherForecastDto>>("api/WeatherForecast", cancellationToken);
        return response ?? Array.Empty<WeatherForecastDto>();
    }
}
