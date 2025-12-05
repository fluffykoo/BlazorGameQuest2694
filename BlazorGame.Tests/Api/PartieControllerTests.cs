using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorGame.Api.Controllers;
using BlazorGame.Api.Data;
using BlazorGame.Api.Services;
using BlazorGame.Domain;
using BlazorGame.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BlazorGame.Tests.Api;

public class PartieControllerTests : DbTestBase
{
    [Fact]
    public async Task DemarrerPartie_CreePartie()
    {
        using var context = NewContext();
        SeedBasicData(context);
        var joueur = context.Joueurs.First();
        var controller = new PartieController(context, new FakeDonjonGenerator());

        var result = await controller.DemarrerPartie(joueur.Id, System.Threading.CancellationToken.None);
        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var partie = Assert.IsType<Partie>(ok.Value);

        Assert.Equal(joueur.Id, partie.JoueurId);
    }

    [Fact]
    public async Task CrudAndQueries_Work()
    {
        using var context = NewContext();
        SeedBasicData(context);
        var joueur = context.Joueurs.First();

        var partie = new Partie { JoueurId = joueur.Id, DonjonId = context.Donjons.First().Id };
        context.Parties.Add(partie);
        await context.SaveChangesAsync();

        var controller = new PartieController(context, new FakeDonjonGenerator());

        var getResult = await controller.GetPartie(partie.Id);
        Assert.Equal(partie.Id, getResult.Value?.Id);

        var listByJoueur = await controller.GetPartiesByJoueur(joueur.Id);
        Assert.Single(listByJoueur.Value!);

        var enCours = await controller.GetPartieEnCours(joueur.Id);
        Assert.Equal(partie.Id, enCours.Value?.Id);

        partie.ScoreFinal = 42;
        var put = await controller.PutPartie(partie.Id, partie);
        Assert.IsType<NoContentResult>(put);

        var delete = await controller.DeletePartie(partie.Id);
        Assert.IsType<NoContentResult>(delete);
        Assert.Empty(context.Parties);
    }

    [Fact]
    public async Task DemarrerPartie_JoueurInexistant_RetourneNotFound()
    {
        using var context = NewContext();
        var controller = new PartieController(context, new DonjonGenerator(context, null, new Random(1)));

        var result = await controller.DemarrerPartie(Guid.NewGuid(), System.Threading.CancellationToken.None);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DemarrerPartie_JoueurDesactive_RetourneBadRequest()
    {
        using var context = NewContext();
        var joueur = new Joueur { Nom = "Dormant", Mail = "d@test.com", EstActif = false };
        context.Joueurs.Add(joueur);
        context.SaveChanges();

        var controller = new PartieController(context, new FakeDonjonGenerator());

        var result = await controller.DemarrerPartie(joueur.Id, System.Threading.CancellationToken.None);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task DemarrerPartie_GenerateurLanceException_Retourne500()
    {
        using var context = NewContext();
        var joueur = new Joueur { Nom = "Hero", Mail = "hero@test.com" };
        context.Joueurs.Add(joueur);
        context.SaveChanges();

        var controller = new PartieController(context, new ThrowingDonjonGenerator());

        var result = await controller.DemarrerPartie(joueur.Id, System.Threading.CancellationToken.None);
        var status = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, status.StatusCode);
    }

    [Fact]
    public async Task PutPartie_IdDifferent_RetourneBadRequest()
    {
        using var context = NewContext();
        var partie = new Partie { Id = Guid.NewGuid(), JoueurId = Guid.NewGuid() };
        context.Parties.Add(partie);
        await context.SaveChangesAsync();

        var controller = new PartieController(context, new FakeDonjonGenerator());
        var response = await controller.PutPartie(Guid.NewGuid(), partie);

        Assert.IsType<BadRequestResult>(response);
    }

    [Fact]
    public async Task DeletePartie_Inexistante_RetourneNotFound()
    {
        using var context = NewContext();
        var controller = new PartieController(context, new FakeDonjonGenerator());

        var response = await controller.DeletePartie(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(response);
    }

    private sealed class ThrowingDonjonGenerator : IDonjonGenerator
    {
        public Task<Partie> DemarrerPartieAsync(Guid joueurId, System.Threading.CancellationToken cancellationToken = default)
        {
            throw new Exception("fail");
        }
    }
}
