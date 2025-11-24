using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorGame.Client;
using BlazorGame.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//httpClient configuré sur l'api
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5040/") });
builder.Services.AddScoped<PlayerState>();

await builder.Build().RunAsync();
