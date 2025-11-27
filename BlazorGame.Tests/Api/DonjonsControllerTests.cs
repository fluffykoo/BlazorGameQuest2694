using System;
using System.Collections.Generic;
using BlazorGame.Api.Controllers;
using BlazorGame.Api.Data;
using BlazorGame.Domain;
using BlazorGame.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BlazorGame.Tests.Api;

public class DonjonsControllerTests : DbTestBase
{
    [Fact]
    public void GetAll_ReturnsDonjons()
    {
        using var context = NewContext();
        SeedBasicData(context);
        var controller = new DonjonsController(context);

        var result = controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var donjons = Assert.IsAssignableFrom<List<Donjon>>(ok.Value);

        Assert.Single(donjons);
        Assert.Equal("Donjon Test", donjons[0].Nom);
    }

    [Fact]
    public void Crud_Works()
    {
        using var context = NewContext();
        var controller = new DonjonsController(context);

        var donjon = new Donjon { Nom = "D1", Description = "desc", NombreDeSalles = 3 };
        var create = controller.Create(donjon);
        Assert.IsType<CreatedAtActionResult>(create);

        var fetched = controller.GetById(donjon.Id);
        var ok = Assert.IsType<OkObjectResult>(fetched);
        var donjonResult = Assert.IsType<Donjon>(ok.Value);
        Assert.Equal("D1", donjonResult.Nom);

        donjon.Nom = "D2";
        var update = controller.Update(donjon.Id, donjon);
        Assert.IsType<NoContentResult>(update);

        var delete = controller.Delete(donjon.Id);
        Assert.IsType<NoContentResult>(delete);
        Assert.Empty(context.Donjons);
    }
}
