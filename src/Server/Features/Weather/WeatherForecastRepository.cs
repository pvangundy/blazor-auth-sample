using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Logging;
using Server.Infrastructure.Data;
using Shared.Features.Weather;

namespace Server.Features.Weather;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private const string DefaultQuery = @"
        WITH Days AS (
            SELECT TOP (@Days) ROW_NUMBER() OVER (ORDER BY (SELECT 1)) - 1 AS DayOffset
            FROM sys.objects
        )
        SELECT 
            CAST(DATEADD(day, DayOffset, SYSUTCDATETIME()) AS date) AS Date,
            CAST(RAND(CHECKSUM(NEWID())) * 40 - 10 AS int) AS TemperatureC,
            'Partly cloudy' AS Summary
        FROM Days";

    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<WeatherForecastRepository> _logger;

    public WeatherForecastRepository(IDbConnectionFactory connectionFactory, ILogger<WeatherForecastRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(int days, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            if (connection is DbConnection dbConnection)
            {
                await dbConnection.OpenAsync(cancellationToken);
            }
            else
            {
                connection.Open();
            }

            var results = await connection.QueryAsync<WeatherForecast>(DefaultQuery, new { Days = days });
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falling back to sample weather data because the database query failed.");
            return Enumerable.Range(1, days).Select(index =>
                new WeatherForecast(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(index)), Random.Shared.Next(-20, 55), "Sample"));
        }
    }
}
