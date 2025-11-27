using System;
using System.Collections.Generic;
using System.Linq;
using BlazorGame.Api.Controllers;
using BlazorGame.Api.Data;
using BlazorGame.Domain;
using BlazorGame.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BlazorGame.Tests.Api;

public class JoueursControllerTests : DbTestBase
{
    [Fact]
    public void GetAll_ReturnsJoueurs()
    {
        using var context = NewContext();
        SeedBasicData(context);
        var controller = new JoueursController(context);

        var result = controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var joueurs = Assert.IsAssignableFrom<List<Joueur>>(ok.Value);

        Assert.Single(joueurs);
        Assert.Equal("TestJoueur", joueurs[0].Nom);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenMissing()
    {
        using var context = NewContext();
        var controller = new JoueursController(context);

        var result = controller.GetById(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Update_Delete_Work()
    {
        using var context = NewContext();
        var controller = new JoueursController(context);

        var joueur = new Joueur { Nom = "Nouveau", Mail = "n@test.com" };
        var created = controller.Create(joueur);
        var createdResult = Assert.IsType<CreatedAtActionResult>(created);
        var createdEntity = Assert.IsType<Joueur>(createdResult.Value);

        createdEntity.Nom = "Maj";
        createdEntity.ScoreTotal = 10;
        var update = controller.Update(createdEntity.Id, createdEntity);
        Assert.IsType<NoContentResult>(update);

        var delete = controller.Delete(createdEntity.Id);
        Assert.IsType<NoContentResult>(delete);
        Assert.Empty(context.Joueurs);
    }
}
