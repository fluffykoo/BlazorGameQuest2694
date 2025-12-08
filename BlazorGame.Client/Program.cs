using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorGame.Client;
using BlazorGame.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// -------------------------------
// API HTTP CLIENT AUTHENTIFIÉ
// -------------------------------
var apiBase = builder.Configuration["Api:BaseAddress"] ?? "http://localhost:5040/";

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(apiBase);
})
.AddHttpMessageHandler<ApiAuthorizationMessageHandler>();
builder.Services.AddScoped<ApiAuthorizationMessageHandler>();

// HttpClient utilisé automatiquement par injection
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

// HttpClient pour Blazor (OIDC, assets)
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// -------------------------------
// CONFIG OIDC (Keycloak)
// -------------------------------
builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority =
        builder.Configuration["Oidc:Authority"] ?? "http://localhost:8080/realms/GameQuest";

    options.ProviderOptions.ClientId =
        builder.Configuration["Oidc:ClientId"] ?? "blazor-client";

    options.ProviderOptions.ResponseType = "code";

    // ------------- IMPORTANT : CALLBACKS OBLIGATOIRES -------------
    options.ProviderOptions.RedirectUri =
        "http://localhost:5000/authentication/login-callback";

    options.ProviderOptions.PostLogoutRedirectUri =
        "http://localhost:5000/authentication/logout-callback";
    // --------------------------------------------------------------

    // Scopes standard + scope pour audience si nécessaire
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");

    // Claims utilisés côté Blazor
    options.UserOptions.RoleClaim = "roles";
    options.UserOptions.NameClaim = "preferred_username";
});

// -------------------------------
// État du joueur côté client
// -------------------------------
builder.Services.AddScoped<PlayerState>();

await builder.Build().RunAsync();
