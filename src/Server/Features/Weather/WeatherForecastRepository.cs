using BlazorAuthSample.Server.Infrastructure.Data;
using BlazorAuthSample.Shared;
using Dapper;

namespace BlazorAuthSample.Server.Features.Weather;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public WeatherForecastRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<WeatherForecastDto>> GetAsync(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var command = new CommandDefinition(
            commandText: "SELECT TOP 5 [Date], [TemperatureC], [Summary] FROM dbo.WeatherForecast ORDER BY [Date]",
            cancellationToken: cancellationToken);

        var records = await connection.QueryAsync<WeatherForecastRecord>(command);

        return records.Select(record => new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(record.Date),
            TemperatureC = record.TemperatureC,
            Summary = record.Summary
        }).ToList();
    }

    private sealed record WeatherForecastRecord(DateTime Date, int TemperatureC, string Summary);
}
