using System.Net.Http.Json;
using Shared.Features.Weather;

namespace Client.Features.Weather;

public class WeatherForecastClient
{
    private readonly HttpClient _httpClient;

    public WeatherForecastClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("api/weather", cancellationToken);
        return response ?? Enumerable.Empty<WeatherForecast>();
    }
}
