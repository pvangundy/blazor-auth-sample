using BlazorAuthSample.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace BlazorAuthSample.Server.Features.Weather;

public static class WeatherForecastEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/weather")
            .RequireAuthorization();

        group.MapGet("/", async (IWeatherForecastRepository repository, CancellationToken cancellationToken) =>
        {
            var forecasts = await repository.GetAsync(cancellationToken);
            return Results.Ok(forecasts);
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

        return builder;
    }
}
