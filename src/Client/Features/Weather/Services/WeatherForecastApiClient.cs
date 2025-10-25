using BlazorAuthSample.Shared;
using System.Net.Http.Json;

namespace BlazorAuthSample.Client.Features.Weather.Services;

public class WeatherForecastApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherForecastApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<WeatherForecastDto>> GetForecastAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<IReadOnlyList<WeatherForecastDto>>("api/weather", cancellationToken);
        return response ?? Array.Empty<WeatherForecastDto>();
    }
}
