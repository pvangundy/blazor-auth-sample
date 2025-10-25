using BlazorAuthSample.Client;
using BlazorAuthSample.Client.Features.Weather;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var backendBaseAddress = builder.Configuration["Backend:BaseAddress"] ?? "https://localhost:5001/";

builder.Services.AddHttpClient("ServerAPI", client => client.BaseAddress = new Uri(backendBaseAddress))
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetRequiredService<BaseAddressAuthorizationMessageHandler>();
        handler.ConfigureHandler(authorizedUrls: new[] { backendBaseAddress });
        return handler;
    });

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));

builder.Services.AddScoped<IWeatherApiClient, WeatherApiClient>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    var defaultScope = builder.Configuration["AzureAd:DefaultScope"];
    if (!string.IsNullOrWhiteSpace(defaultScope))
    {
        options.ProviderOptions.DefaultAccessTokenScopes.Add(defaultScope);
    }
    options.ProviderOptions.LoginMode = "redirect";
});

await builder.Build().RunAsync();
