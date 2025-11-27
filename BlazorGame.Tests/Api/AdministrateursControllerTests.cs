using System;
using System.Collections.Generic;
using BlazorGame.Api.Controllers;
using BlazorGame.Api.Data;
using BlazorGame.Domain;
using BlazorGame.Tests.Common;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BlazorGame.Tests.Api;

public class AdministrateursControllerTests : DbTestBase
{
    [Fact]
    public void GetAll_ReturnsAdmins()
    {
        using var context = NewContext();
        SeedBasicData(context);
        var controller = new AdministrateursController(context);

        var result = controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var admins = Assert.IsAssignableFrom<List<Administrateur>>(ok.Value);

        Assert.Single(admins);
        Assert.Equal("admin", admins[0].NomUtilisateur);
    }

    [Fact]
    public void Crud_Works()
    {
        using var context = NewContext();
        var controller = new AdministrateursController(context);

        var admin = new Administrateur { NomUtilisateur = "adm", Email = "adm@test.com", MotDePasse = "pwd" };
        var create = controller.Create(admin);
        Assert.IsType<CreatedAtActionResult>(create);

        var fetched = controller.GetById(admin.Id);
        var ok = Assert.IsType<OkObjectResult>(fetched);
        var adminResult = Assert.IsType<Administrateur>(ok.Value);
        Assert.Equal("adm", adminResult.NomUtilisateur);

        admin.NomUtilisateur = "adm2";
        var update = controller.Update(admin.Id, admin);
        Assert.IsType<NoContentResult>(update);

        var delete = controller.Delete(admin.Id);
        Assert.IsType<NoContentResult>(delete);
        Assert.Empty(context.Administrateurs);
    }
}
