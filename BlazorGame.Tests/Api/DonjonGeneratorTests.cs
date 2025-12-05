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

    [Fact]
    public async Task DemarrerPartie_JoueurIdVide_DeclencheArgumentException()
    {
        using var context = NewContext();
        var generator = new DonjonGenerator(context, new[] { new DonjonTemplate("T", "d", 1, 1) }, new Random(1));

        await Assert.ThrowsAsync<ArgumentException>(() => generator.DemarrerPartieAsync(Guid.Empty));
    }

    [Fact]
    public async Task DemarrerPartie_JoueurAbsent_DeclencheInvalidOperationException()
    {
        using var context = NewContext();
        var generator = new DonjonGenerator(context, new[] { new DonjonTemplate("T", "d", 1, 1) }, new Random(1));

        await Assert.ThrowsAsync<InvalidOperationException>(() => generator.DemarrerPartieAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DemarrerPartie_GenereSallesAvecStatsEtDifficulte()
    {
        using var context = NewContext();
        var joueur = new Joueur { Id = Guid.NewGuid(), Nom = "Hero", Mail = "hero@test.com" };
        context.Joueurs.Add(joueur);
        context.SaveChanges();

        var template = new DonjonTemplate("Full", "desc", 5, 5);
        var generator = new DonjonGenerator(context, new[] { template }, new Random(1));

        var partie = await generator.DemarrerPartieAsync(joueur.Id);

        Assert.Equal(5, partie.Salles.Count);
        Assert.Equal(Enumerable.Range(1, 5), partie.Salles.Select(s => s.Position));
        Assert.All(partie.Salles, s =>
        {
            if (s.ChoixPossible.Contains(ChoixAction.Combattre))
            {
                Assert.False(string.IsNullOrWhiteSpace(s.NomMonstre));
                Assert.False(string.IsNullOrWhiteSpace(s.ImageMonstre));
                Assert.True(s.PvMonstre >= 12); // 8 + (pos * 4) min
                Assert.True(s.ForceMonstre >= 3); // 1 + (pos * 2) min
            }
            else
            {
                Assert.False(string.IsNullOrWhiteSpace(s.Description));
            }
        });

        Assert.Equal(NiveauDifficulte.Facile, partie.Salles[0].Niveau);
        Assert.Equal(NiveauDifficulte.Facile, partie.Salles[1].Niveau);
        Assert.Equal(NiveauDifficulte.Moyen, partie.Salles[2].Niveau);
        Assert.Equal(NiveauDifficulte.Moyen, partie.Salles[3].Niveau);
        Assert.Equal(NiveauDifficulte.Difficile, partie.Salles[4].Niveau);
    }

    [Fact]
    public async Task DemarrerPartie_ContientCombatEtCoffre()
    {
        using var context = NewContext();
        var joueur = new Joueur { Id = Guid.NewGuid(), Nom = "Mix", Mail = "mix@test.com" };
        context.Joueurs.Add(joueur);
        context.SaveChanges();

        var template = new DonjonTemplate("Mix", "desc", 4, 4);
        var generator = new DonjonGenerator(context, new[] { template }, new Random(123));

        var partie = await generator.DemarrerPartieAsync(joueur.Id);

        var combats = partie.Salles.Count(s => s.ChoixPossible.Contains(ChoixAction.Combattre));
        var coffres = partie.Salles.Count(s => !s.ChoixPossible.Contains(ChoixAction.Combattre));

        Assert.True(combats > coffres);
        Assert.True(coffres >= 1);
    }
}
