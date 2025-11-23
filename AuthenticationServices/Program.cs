using AuthenticationServices.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la base de données PostgreSQL ---
builder.Services.AddDbContext<AventureDbContext>(options =>
    options.UseNpgsql("Host=127.0.0.1;Port=5432;Database=AventureDB;Username=postgres;Password=postgres"));

// CORS : autoriser le client Blazor (http://localhost:5000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:5000")   // ton client Blazor
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Ajout des contrôleurs et Swagger + gestion des cycles JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuration du pipeline HTTP 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Active la policy CORS définie plus haut
app.UseCors("AllowBlazorClient");

app.UseAuthorization();

app.MapControllers(); // pour activer tes endpoints d’API

app.Run();