using Microsoft.EntityFrameworkCore;
using Models;

namespace AuthenticationServices.Data;

public class AventureDbContext : DbContext
{
    public AventureDbContext(DbContextOptions<AventureDbContext> options)
        : base(options)
    {
    }

    // Tables du jeu
    public DbSet<Joueur> Joueurs { get; set; }
    public DbSet<Partie> Parties { get; set; }
    public DbSet<Salle> Salles { get; set; }
    public DbSet<Donjon> Donjons { get; set; }
    public DbSet<Administrateur> Administrateurs { get; set; }

    //  Configuration des relations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Joueur → Parties
        modelBuilder.Entity<Joueur>()
            .HasMany(j => j.Historique)
            .WithOne(p => p.Joueur)
            .HasForeignKey(p => p.JoueurId)
            .OnDelete(DeleteBehavior.Cascade);

        // Partie → Salles
        modelBuilder.Entity<Partie>()
            .HasMany(p => p.Salles)
            .WithOne(s => s.Partie)
            .HasForeignKey(s => s.PartieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}