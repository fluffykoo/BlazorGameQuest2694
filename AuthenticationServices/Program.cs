using AuthenticationServices.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Configuration de la base de données PostgreSQL ---
builder.Services.AddDbContext<AventureDbContext>(options =>
    options.UseNpgsql("Host=127.0.0.1;Port=5432;Database=AventureDB;Username=postgres;Password=postgres"));

//Ajout des contrôleurs et Swagger 
// gestion des cycles JSON au lieu du JSON ignore 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true; // pour avoir du JSON lisible
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
app.UseAuthorization();

app.MapControllers(); // pour activer tes endpoints d’API

app.Run();