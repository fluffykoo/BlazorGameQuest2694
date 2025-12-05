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

public class SalleControllerTests : DbTestBase
{
    [Fact]
    public async Task GetSalles_ReturnsList()
    {
        using var context = NewContext();
        SeedBasicData(context);
        var joueur = context.Joueurs.First();
        var partie = new Partie { Id = Guid.NewGuid(), JoueurId = joueur.Id };
        context.Parties.Add(partie);
        context.Salles.Add(new Salle
        {
            Id = Guid.NewGuid(),
            PartieId = partie.Id,
            Position = 1,
            Description = "Salle test",
            Niveau = NiveauDifficulte.Facile,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Combattre }
        });
        context.SaveChanges();

        var controller = new SalleController(context);
        var actionResult = await controller.GetSalles();

        var salles = Assert.IsAssignableFrom<IEnumerable<Salle>>(actionResult.Value);
        Assert.Single(salles);
    }

    [Fact]
    public async Task ExecuterAction_RetourneResultat()
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
            Description = "Salle test",
            Niveau = NiveauDifficulte.Moyen,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Combattre, ChoixAction.Fouiller }
        };
        context.Salles.Add(salle);
        context.SaveChanges();

        var controller = new SalleController(context);
        var actionResult = await controller.ExecuterAction(salle.Id, ChoixAction.Combattre);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var resultat = Assert.IsType<ActionResultat>(okResult.Value);
        Assert.NotNull(resultat);
    }

    [Fact]
    public async Task Crud_And_Batch_Work()
    {
        using var context = NewContext();
        SeedBasicData(context);
        var joueur = context.Joueurs.First();
        var partie = new Partie { JoueurId = joueur.Id, DonjonId = context.Donjons.First().Id };
        context.Parties.Add(partie);
        await context.SaveChangesAsync();

        var controller = new SalleController(context);

        // Batch
        var partieId = partie.Id;
        var salles = new List<Salle>
        {
            new Salle { Id = Guid.NewGuid(), PartieId = partieId, Position = 1 },
            new Salle { Id = Guid.NewGuid(), PartieId = partieId, Position = 2 }
        };
        var batch = await controller.PostSalles(salles);
        Assert.IsType<OkResult>(batch);
        Assert.Equal(2, context.Salles.Count());

        // Post
        var salle = new Salle
        {
            PartieId = partie.Id,
            Position = 3,
            Description = "CRUD salle",
            Niveau = NiveauDifficulte.Facile,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Combattre }
        };
        var created = await controller.PostSalle(salle);
        Assert.IsType<CreatedAtActionResult>(created.Result);

        var fetched = await controller.GetSalle(salle.Id);
        Assert.Equal("CRUD salle", fetched.Value?.Description);

        var list = await controller.GetSallesByPartie(partie.Id);
        Assert.True((list.Value?.Count() ?? 0) >= 3);

        salle.Description = "Maj";
        var put = await controller.PutSalle(salle.Id, salle);
        Assert.IsType<NoContentResult>(put);

        var delete = await controller.DeleteSalle(salle.Id);
        Assert.IsType<NoContentResult>(delete);
    }

    [Fact]
    public async Task NotFound_And_BadRequest()
    {
        using var context = NewContext();
        var controller = new SalleController(context);

        var get = await controller.GetSalle(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(get.Result);

        var salle = new Salle { Id = Guid.NewGuid(), PartieId = Guid.NewGuid(), Position = 1 };
        var badPut = await controller.PutSalle(Guid.NewGuid(), salle);
        Assert.IsType<BadRequestResult>(badPut);
    }

    [Fact]
    public async Task ScoreNegatif_TerminePartie()
    {
        using var context = NewContext();
        SeedBasicData(context);

        var joueur = context.Joueurs.First();
        var partie = new Partie
        {
            JoueurId = joueur.Id,
            DonjonId = context.Donjons.First().Id,
            ScoreFinal = -1000,
            EstTerminee = false
        };
        context.Parties.Add(partie);

        var salle = new Salle
        {
            Id = Guid.NewGuid(),
            PartieId = partie.Id,
            Position = 1,
            Description = "Salle test",
            Niveau = NiveauDifficulte.Facile,
            ChoixPossible = new List<ChoixAction> { ChoixAction.Fouiller }
        };
        context.Salles.Add(salle);
        await context.SaveChangesAsync();

        var controller = new SalleController(context);
        var actionResult = await controller.ExecuterAction(salle.Id, ChoixAction.Fouiller);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var resultat = Assert.IsType<ActionResultat>(okResult.Value);

        Assert.True(partie.EstTerminee);
        Assert.True(resultat.ScoreTotal <= 0);
        Assert.Equal(partie.ScoreFinal, resultat.ScoreTotal);
        Assert.Equal(partie.ScoreFinal, context.Joueurs.Find(joueur.Id)!.ScoreTotal);
    }
}
