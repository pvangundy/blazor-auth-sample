using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Authentication.WebAssembly.Msal;
using Client;
using Client.Features.Weather;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("ServerAPI", client =>
{
    var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? builder.HostEnvironment.BaseAddress;
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => new WeatherForecastClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI")));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    var defaultScope = builder.Configuration["AzureAd:DefaultScope"];
    if (!string.IsNullOrWhiteSpace(defaultScope))
    {
        options.ProviderOptions.DefaultAccessTokenScopes.Add(defaultScope);
    }
    var additionalScope = builder.Configuration["AzureAd:AdditionalScope"];
    if (!string.IsNullOrWhiteSpace(additionalScope))
    {
        options.ProviderOptions.AdditionalScopesToConsent.Add(additionalScope);
    }
    options.ProviderOptions.LoginMode = "redirect";
});

await builder.Build().RunAsync();
