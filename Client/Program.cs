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

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var client = factory.CreateClient();
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    return client;
});

builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();
