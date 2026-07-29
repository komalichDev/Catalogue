using Client;
using Client.Helpers;
using Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<IHttpRequestExecuter, HttpRequestExecuter>();
builder.Services.AddScoped<IHttpProductApi, HttpProductApi>();

builder.Services.AddHttpClient();

var apiBaseAddress = builder.Configuration["ApiBaseAddress"];

if (string.IsNullOrEmpty(apiBaseAddress))
{
    apiBaseAddress = builder.HostEnvironment.BaseAddress;
}

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient();

    client.BaseAddress = new Uri(apiBaseAddress);
    return client;
});

builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();
