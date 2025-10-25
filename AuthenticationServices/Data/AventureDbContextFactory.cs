/* using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthenticationServices.Data;

public class AventureDbContextFactory : IDesignTimeDbContextFactory<AventureDbContext>
{
    public AventureDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AventureDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AventureDB;Username=postgres;Password=postgres");

        return new AventureDbContext(optionsBuilder.Options);
    }
} */
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace AuthenticationServices.Data;

public class AventureDbContextFactory : IDesignTimeDbContextFactory<AventureDbContext>
{
    public AventureDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AventureDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=127.0.0.1;Port=5432;Database=AventureDB;Username=postgres;Password=postgres");

        // Test direct de connexion
        try
        {
            using var conn = new Npgsql.NpgsqlConnection(
                "Host=127.0.0.1;Port=5432;Database=AventureDB;Username=postgres;Password=postgres");
            conn.Open();
            Console.WriteLine("✅ Connexion PostgreSQL réussie !");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Erreur connexion PostgreSQL : " + ex.Message);
        }

        return new AventureDbContext(optionsBuilder.Options);
    }
}