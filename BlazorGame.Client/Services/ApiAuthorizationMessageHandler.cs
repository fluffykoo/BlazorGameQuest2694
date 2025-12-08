using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorGame.Client.Services;

/// <summary>
/// Force l'ajout du bearer pour les appels vers l'API backend.
/// </summary>
public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager)
        : base(provider, navigationManager)
    {
        ConfigureHandler(authorizedUrls: new[] { "http://localhost:5040" });
    }
}
