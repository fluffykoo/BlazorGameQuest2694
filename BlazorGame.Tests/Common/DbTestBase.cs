using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorGame.Api.Data;
using BlazorGame.Api.Services;
using BlazorGame.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorGame.Tests.Common;

public abstract class DbTestBase
{
    protected AventureDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AventureDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AventureDbContext(options);
    }

    protected void SeedBasicData(AventureDbContext context)
    {
        var joueur = new Joueur { Id = Guid.NewGuid(), Nom = "TestJoueur", Mail = "test@example.com", ScoreTotal = 100 };
        var admin = new Administrateur { Id = Guid.NewGuid(), NomUtilisateur = "admin", Email = "admin@test.com", MotDePasse = "pwd" };
        var donjon = new Donjon { Id = Guid.NewGuid(), Nom = "Donjon Test", Description = "Desc", NombreDeSalles = 5 };

        context.Joueurs.Add(joueur);
        context.Administrateurs.Add(admin);
        context.Donjons.Add(donjon);
        context.SaveChanges();
    }

    protected class FakeDonjonGenerator : IDonjonGenerator
    {
        public Task<Partie> DemarrerPartieAsync(Guid joueurId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new Partie
            {
                Id = Guid.NewGuid(),
                JoueurId = joueurId,
                DonjonId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                EstTerminee = false,
                ScoreFinal = 0,
                Salles = new List<Salle>()
            });
        }
    }
}
