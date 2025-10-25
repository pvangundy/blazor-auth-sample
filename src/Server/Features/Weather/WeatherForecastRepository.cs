using System.Linq;
using BlazorAuthSample.Server.Infrastructure.Data;
using BlazorAuthSample.Shared.Weather;
using Dapper;

namespace BlazorAuthSample.Server.Features.Weather;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public WeatherForecastRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<WeatherForecastDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT [Date], [TemperatureC], [Summary] FROM WeatherForecast";

        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<WeatherForecastDto>(new CommandDefinition(sql, cancellationToken: cancellationToken));
        return results.ToList();
    }
}
