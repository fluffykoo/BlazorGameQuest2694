using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BlazorGame.Api.Controllers;
using BlazorGame.Api.Data;
using BlazorGame.Domain;
using BlazorGame.Api.GameConfig;
using BlazorGame.Api.Services;

using System.Threading.Tasks;

using Xunit;

namespace BlazorGame.Tests
{
    public class ControllerTests
    {
        private AventureDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AventureDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AventureDbContext(options);
        }

        private void SeedTestData(AventureDbContext context)
        {
            // Joueur test
            var joueur = new Joueur
            {
                Id = Guid.NewGuid(),
                Nom = "TestJoueur",
                Mail = "test@example.com",
                ScoreTotal = 100
            };
            context.Joueurs.Add(joueur);

            // Admin test
            var admin = new Administrateur
            {
                Id = Guid.NewGuid(),
                NomUtilisateur = "admin",
                Email = "admin@test.com",
                MotDePasse = "password"
            };
            context.Administrateurs.Add(admin);

            // Donjon test
            var donjon = new Donjon
            {
                Id = Guid.NewGuid(),
                Nom = "Donjon Test",
                Description = "Description test",
                NombreDeSalles = 5
            };
            context.Donjons.Add(donjon);

            context.SaveChanges();
        }

        [Fact]
        public void Test_JoueursController_GetAll()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var controller = new JoueursController(context);

            var result = controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var joueurs = Assert.IsAssignableFrom<List<Joueur>>(okResult.Value);

            Assert.Single(joueurs);
            Assert.Equal("TestJoueur", joueurs[0].Nom);
        }

        [Fact]
        public void Test_JoueursController_GetById()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var controller = new JoueursController(context);
            var joueurId = context.Joueurs.First().Id;

            var result = controller.GetById(joueurId);

            var ok = Assert.IsType<OkObjectResult>(result);
            var joueur = Assert.IsType<Joueur>(ok.Value);

            Assert.Equal(joueurId, joueur.Id);
        }

        [Fact]
        public void Test_JoueursController_Create()
        {
            using var context = GetInMemoryDbContext();

            var controller = new JoueursController(context);

            var newJoueur = new Joueur
            {
                Nom = "NouveauJoueur",
                Mail = "nouveau@test.com",
                ScoreTotal = 0
            };

            var result = controller.Create(newJoueur);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var joueur = Assert.IsType<Joueur>(created.Value);

            Assert.Equal("NouveauJoueur", joueur.Nom);
            Assert.Single(context.Joueurs);
        }

       [Fact]
        public async Task Test_PartieController_GetParties()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var joueur = context.Joueurs.First();

            var templates = new List<DonjonTemplate>
            {
                new DonjonTemplate("Donjon Test Unitaire", "Template pour tests", 1, 1)
            };

            var generator = new DonjonGenerator(context, templates, new Random(42));
            var controller = new PartieController(context, generator);

            // On lance une partie
            var demarrerResult = await controller.DemarrerPartie(joueur.Id);

            // Récupération du OkObjectResult :
            var ok = Assert.IsType<OkObjectResult>(demarrerResult.Result);

            var partieCreee = Assert.IsType<Partie>(ok.Value);

            Assert.Equal(joueur.Id, partieCreee.JoueurId);

            // On teste GetParties
            var actionResult = await controller.GetParties();

            var parties = Assert.IsAssignableFrom<IEnumerable<Partie>>(actionResult.Value);

            Assert.Single(parties);
        }

       [Fact]
        public async Task Test_SalleController_GetSalles()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var joueur = context.Joueurs.First();

            var partie = new Partie
            {
                Id = Guid.NewGuid(),
                JoueurId = joueur.Id
            };
            context.Parties.Add(partie);

            var salle = new Salle
            {
                Id = Guid.NewGuid(),
                PartieId = partie.Id,
                Position = 1,
                Description = "Salle test",
                Niveau = NiveauDifficulte.Facile,
                ChoixPossible = new List<ChoixAction> { ChoixAction.Combattre }
            };
            context.Salles.Add(salle);
            context.SaveChanges();

            var controller = new SalleController(context);

            // Act
            var actionResult = await controller.GetSalles(); // ActionResult<IEnumerable<Salle>>

            // Assert
            var salles = Assert.IsAssignableFrom<IEnumerable<Salle>>(actionResult.Value);
            Assert.Single(salles);
            Assert.Equal("Salle test", salles.First().Description);
        }

        [Fact]
        public void Test_AdministrateursController_GetAll()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var controller = new AdministrateursController(context);

            var result = controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var admins = Assert.IsAssignableFrom<List<Administrateur>>(ok.Value);

            Assert.Single(admins);
            Assert.Equal("admin", admins[0].NomUtilisateur);
        }

        [Fact]
        public void Test_DonjonsController_GetAll()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var controller = new DonjonsController(context);

            var result = controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var donjons = Assert.IsAssignableFrom<List<Donjon>>(ok.Value);

            Assert.Single(donjons);
            Assert.Equal("Donjon Test", donjons[0].Nom);
        }

        [Fact]
        public void Test_JoueursController_Update()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var controller = new JoueursController(context);

            var joueurId = context.Joueurs.First().Id;

            var updated = new Joueur
            {
                Id = joueurId,
                Nom = "JoueurModifié",
                Mail = "modifie@test.com",
                ScoreTotal = 200
            };

            var result = controller.Update(joueurId, updated);

            Assert.IsType<NoContentResult>(result);

            var joueur = context.Joueurs.Find(joueurId);

            Assert.Equal("JoueurModifié", joueur.Nom);
            Assert.Equal(200, joueur.ScoreTotal);
        }

        [Fact]
        public void Test_JoueursController_Delete()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var controller = new JoueursController(context);

            var joueurId = context.Joueurs.First().Id;

            var result = controller.Delete(joueurId);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Joueurs);
        }

        [Fact]
        public async Task Test_SalleController_ExecuterAction()
        {
            using var context = GetInMemoryDbContext();
            SeedTestData(context);

            var joueur = context.Joueurs.First();

            var partie = new Partie
            {
                Id = Guid.NewGuid(),
                JoueurId = joueur.Id
            };

            context.Parties.Add(partie);

            var salle = new Salle
            {
                Id = Guid.NewGuid(),
                PartieId = partie.Id,
                Position = 1,
                Description = "Salle test",
                Niveau = NiveauDifficulte.Moyen,
                ChoixPossible = new List<ChoixAction>
                {
                    ChoixAction.Combattre,
                    ChoixAction.Fouiller
                }
            };

            context.Salles.Add(salle);
            context.SaveChanges();

            var controller = new SalleController(context);

            // Act
            var actionResult = await controller.ExecuterAction(salle.Id, ChoixAction.Combattre); // ActionResult<ActionResultat>

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultat = Assert.IsType<ActionResultat>(okResult.Value);
            Assert.NotNull(resultat);
        }
    }
}
