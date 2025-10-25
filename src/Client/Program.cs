using BlazorAuthSample.Client;
using BlazorAuthSample.Client.Features.Weather.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.AddRange(
        builder.Configuration.GetSection("Api:Scopes").Get<string[]>() ?? Array.Empty<string>());
});

builder.Services.AddScoped<WeatherForecastService>();
builder.Services.AddHttpClient<WeatherForecastApiClient>(client =>
    client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"]!))
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { builder.Configuration["Api:BaseUrl"]! },
                scopes: builder.Configuration.GetSection("Api:Scopes").Get<string[]>() ?? Array.Empty<string>());
        return handler;
    });

await builder.Build().RunAsync();
