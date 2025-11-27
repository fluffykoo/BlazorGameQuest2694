using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorGame.Api.GameConfig;
using BlazorGame.Api.Services;
using BlazorGame.Api.Data;
using BlazorGame.Domain;
using BlazorGame.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlazorGame.Tests.Api;

public class DonjonGeneratorTests : DbTestBase
{
    [Fact]
    public async Task Generateur_RespecteIntervalleEtMonstres()
    {
        using var context = NewContext();
        var joueur = new Joueur { Id = Guid.NewGuid(), Nom = "Hero", Mail = "hero@test.com" };
        context.Joueurs.Add(joueur);
        context.SaveChanges();

        var templates = new[] { new DonjonTemplate("Test", "desc", 2, 3) };
        var generator = new DonjonGenerator(context, templates, new Random(1));

        var partie = await generator.DemarrerPartieAsync(joueur.Id);

        Assert.InRange(partie.Donjon!.NombreDeSalles, 2, 3);
        Assert.Equal(partie.Donjon.NombreDeSalles, partie.Salles.Count);
        Assert.All(partie.Salles, s => Assert.False(string.IsNullOrWhiteSpace(s.NomMonstre)));
    }

    [Fact]
    public async Task DemarrerPartie_CreeSalles_OrdreEtFk()
    {
        using var context = NewContext();
        var joueur = new Joueur { Id = Guid.NewGuid(), Nom = "Testeur", Mail = "t@test.com" };
        context.Joueurs.Add(joueur);
        context.SaveChanges();

        var templates = new List<DonjonTemplate>
        {
            new DonjonTemplate("Donjon Test", "Desc", 2, 4)
        };

        var generator = new DonjonGenerator(context, templates, new Random(42));

        var partie = await generator.DemarrerPartieAsync(joueur.Id);

        Assert.Equal(joueur.Id, partie.JoueurId);
        Assert.NotNull(partie.Donjon);
        Assert.InRange(partie.Donjon!.NombreDeSalles, 2, 4);
        Assert.Equal(partie.Donjon.NombreDeSalles, partie.Salles.Count);
        Assert.True(partie.Salles.Select(s => s.Position).OrderBy(p => p).SequenceEqual(Enumerable.Range(1, partie.Salles.Count)));
    }
}
