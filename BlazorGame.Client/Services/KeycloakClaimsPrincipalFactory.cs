using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace BlazorGame.Client.Services;

/// <summary>
/// Mappe les rôles Keycloak (realm_access / resource_access) vers ClaimTypes.Role et "roles"
/// pour que [Authorize(Roles=...)] et AuthorizeView fonctionnent.
/// </summary>
public class KeycloakClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public KeycloakClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor) { }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);

        if (user.Identity is not ClaimsIdentity identity)
        {
            return user;
        }

        AddRealmRoles(account, identity);
        AddClientRoles(account, identity, "blazorgame-api");

        return user;
    }

    private static void AddRealmRoles(RemoteUserAccount account, ClaimsIdentity identity)
    {
        if (account.AdditionalProperties is null) return;
        if (!account.AdditionalProperties.TryGetValue("realm_access", out var realmObj) || realmObj is null) return;
        if (realmObj is not JsonElement realm) return;
        if (!realm.TryGetProperty("roles", out var rolesElem) || rolesElem.ValueKind != JsonValueKind.Array) return;

        foreach (var role in rolesElem.EnumerateArray())
        {
            var value = role.GetString();
            if (string.IsNullOrWhiteSpace(value)) continue;
            identity.AddClaim(new Claim(ClaimTypes.Role, value));
            identity.AddClaim(new Claim("roles", value));
        }
    }

    private static void AddClientRoles(RemoteUserAccount account, ClaimsIdentity identity, string clientId)
    {
        if (account.AdditionalProperties is null) return;
        if (!account.AdditionalProperties.TryGetValue("resource_access", out var resObj) || resObj is null) return;
        if (resObj is not JsonElement res) return;
        if (!res.TryGetProperty(clientId, out var client) || !client.TryGetProperty("roles", out var clientRoles) || clientRoles.ValueKind != JsonValueKind.Array) return;

        foreach (var role in clientRoles.EnumerateArray())
        {
            var value = role.GetString();
            if (string.IsNullOrWhiteSpace(value)) continue;
            identity.AddClaim(new Claim(ClaimTypes.Role, value));
            identity.AddClaim(new Claim("roles", value));
        }
    }
}
