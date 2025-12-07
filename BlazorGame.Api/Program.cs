using BlazorGame.Api.Data;
using BlazorGame.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// JWT Authentication
// ------------------------------------------------------
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.Authority = builder.Configuration["Jwt:Authority"];
    options.MetadataAddress = builder.Configuration["Jwt:MetadataAddress"];
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Authority"],
        RoleClaimType = "roles",
        NameClaimType = "preferred_username"
    };
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            if (context.Principal?.Identity is not ClaimsIdentity identity)
                return Task.CompletedTask;

            // Ajout des rôles realm_access (Keycloak)
            var realmAccess = context.Principal.FindFirst("realm_access")?.Value;
            if (!string.IsNullOrWhiteSpace(realmAccess))
            {
                using var document = JsonDocument.Parse(realmAccess);
                if (document.RootElement.TryGetProperty("roles", out var rolesElement))
                {
                    foreach (var role in rolesElement.EnumerateArray())
                    {
                        var value = role.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            identity.AddClaim(new Claim(identity.RoleClaimType, value));
                        }
                    }
                }
            }

            // Ajout des rôles côté client (resource_access -> blazorgame-api -> roles)
            var resourceAccess = context.Principal.FindFirst("resource_access")?.Value;
            if (!string.IsNullOrWhiteSpace(resourceAccess))
            {
                using var document = JsonDocument.Parse(resourceAccess);
                if (document.RootElement.TryGetProperty("blazorgame-api", out var clientAccess) &&
                    clientAccess.TryGetProperty("roles", out var clientRoles))
                {
                    foreach (var role in clientRoles.EnumerateArray())
                    {
                        var value = role.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            identity.AddClaim(new Claim(identity.RoleClaimType, value));
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// ------------------------------------------------------
// Base de données: Configuration de la base de données PostgreSQL 
// ------------------------------------------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DB_CONNECTION")
    ?? "Host=127.0.0.1;Port=5432;Database=AventureDB;Username=postgres;Password=postgres";

builder.Services.AddDbContext<AventureDbContext>(options =>
    options.UseNpgsql(connectionString));

// ------------------------------------------------------
// Services métiers
// ------------------------------------------------------
builder.Services.AddScoped<IDonjonGenerator, DonjonGenerator>();

// ------------------------------------------------------
//  CORS : autoriser le client Blazor (http://localhost:5000)
// ------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ------------------------------------------------------
// Controllers & Swagger
// ------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ------------------------------------------------------
// Pipeline HTTP
// ------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

// validation du token JWT
app.UseAuthentication();

//Autorisation selon les rôles
app.UseAuthorization();

app.MapControllers();

app.Run();
