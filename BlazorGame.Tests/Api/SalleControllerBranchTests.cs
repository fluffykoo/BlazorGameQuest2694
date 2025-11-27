using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorGame.Api.Controllers;
using BlazorGame.Api.Data;
using BlazorGame.Domain;
using BlazorGame.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BlazorGame.Tests.Api;

public class SalleControllerBranchTests : DbTestBase
{
    [Fact]
    public async Task ExecuterAction_Combat_VictoireEtDefaite()
    {
        using var context = NewContext();
        SeedBasicData(context);

        var joueur = context.Joueurs.First();
        var partie = new Partie { Id = Guid.NewGuid(), JoueurId = joueur.Id };
        context.Parties.Add(partie);

        var salle = new Salle
        {
            Id = Guid.NewGuid(),
            PartieId = partie.Id,
            Position = 1,
            Description = "Salle combat",
            Niveau = NiveauDifficulte.Moyen,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Combattre }
        };
        context.Salles.Add(salle);
        context.SaveChanges();

        var controller = new SalleController(context);

        // Forcer un premier tirage (on ne contrôle pas le Random, mais on s'assure que l'appel ne jette pas)
        var result = await controller.ExecuterAction(salle.Id, ChoixAction.Combattre);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var res = Assert.IsType<ActionResultat>(ok.Value);
        Assert.Equal(ChoixAction.Combattre, res.Action);
    }

    [Fact]
    public async Task ExecuterAction_Fouille_TresorEtPiege()
    {
        using var context = NewContext();
        SeedBasicData(context);

        var joueur = context.Joueurs.First();
        var partie = new Partie { Id = Guid.NewGuid(), JoueurId = joueur.Id };
        context.Parties.Add(partie);

        var salle = new Salle
        {
            Id = Guid.NewGuid(),
            PartieId = partie.Id,
            Position = 1,
            Description = "Salle fouille",
            Niveau = NiveauDifficulte.Facile,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Fouiller }
        };
        context.Salles.Add(salle);
        context.SaveChanges();

        var controller = new SalleController(context);
        var result = await controller.ExecuterAction(salle.Id, ChoixAction.Fouiller);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var res = Assert.IsType<ActionResultat>(ok.Value);
        Assert.Equal(ChoixAction.Fouiller, res.Action);
    }

    [Fact]
    public async Task ExecuterAction_DerniereSalle_TerminePartie()
    {
        using var context = NewContext();

        var joueur = new Joueur { Id = Guid.NewGuid(), Nom = "Hero", Mail = "hero@test.com" };
        var partie = new Partie { Id = Guid.NewGuid(), JoueurId = joueur.Id };

        var salle1 = new Salle
        {
            Id = Guid.NewGuid(),
            PartieId = partie.Id,
            Position = 1,
            Description = "Salle 1",
            Niveau = NiveauDifficulte.Facile,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Combattre }
        };
        var salle2 = new Salle
        {
            Id = Guid.NewGuid(),
            PartieId = partie.Id,
            Position = 2,
            Description = "Salle 2",
            Niveau = NiveauDifficulte.Moyen,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Fouiller }
        };

        partie.Salles = new List<Salle> { salle1, salle2 };

        context.Joueurs.Add(joueur);
        context.Parties.Add(partie);
        context.Salles.AddRange(salle1, salle2);
        context.SaveChanges();

        var controller = new SalleController(context);

        var actionResult = await controller.ExecuterAction(salle2.Id, ChoixAction.Fouiller);
        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var resultat = Assert.IsType<ActionResultat>(ok.Value);

        Assert.True(salle2.EstVisitee);
        Assert.Equal(ChoixAction.Fouiller, salle2.ChoixFait);
        Assert.True(partie.EstTerminee);
        Assert.Equal(partie.ScoreFinal, resultat.ScoreTotal);
    }
}
