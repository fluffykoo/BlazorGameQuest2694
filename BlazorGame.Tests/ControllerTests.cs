using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationServices.Controllers;
using AuthenticationServices.Data;
using Models;
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
            // Création d'un joueur de test
            var joueur = new Joueur 
            { 
                Id = Guid.NewGuid(),
                Nom = "TestJoueur",
                Mail = "test@example.com",
                ScoreTotal = 100
            };
            context.Joueurs.Add(joueur);

            // Création d'un administrateur de test
            var admin = new Administrateur 
            { 
                Id = Guid.NewGuid(),
                NomUtilisateur = "admin",
                Email = "admin@test.com",
                MotDePasse = "password"
            };
            context.Administrateurs.Add(admin);

            // Création d'un donjon de test
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
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            var controller = new JoueursController(context);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var joueurs = Assert.IsAssignableFrom<List<Joueur>>(okResult.Value);
            Assert.Single(joueurs);
            Assert.Equal("TestJoueur", joueurs[0].Nom);
        }

        [Fact]
        public void Test_JoueursController_GetById()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            var controller = new JoueursController(context);
            var joueurId = context.Joueurs.First().Id;

            // Act
            var result = controller.GetById(joueurId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var joueur = Assert.IsType<Joueur>(okResult.Value);
            Assert.Equal(joueurId, joueur.Id);
        }

        [Fact]
        public void Test_JoueursController_Create()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var controller = new JoueursController(context);
            var newJoueur = new Joueur 
            { 
                Nom = "NouveauJoueur",
                Mail = "nouveau@test.com",
                ScoreTotal = 0
            };

            // Act
            var result = controller.Create(newJoueur);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var joueur = Assert.IsType<Joueur>(createdResult.Value);
            Assert.Equal("NouveauJoueur", joueur.Nom);
            Assert.Single(context.Joueurs);
        }

        [Fact]
        public void Test_PartieController_GetParties()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            
            // Création d'une partie de test
            var joueur = context.Joueurs.First();
            var partie = new Partie
            {
                Id = Guid.NewGuid(),
                JoueurId = joueur.Id,
                EstTerminee = false,
                ScoreFinal = 0
            };
            context.Parties.Add(partie);
            context.SaveChanges();

            var controller = new PartieController(context);

            // Act
            var result = controller.GetParties().Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var parties = Assert.IsAssignableFrom<List<Partie>>(okResult.Value);
            Assert.Single(parties);
        }

        [Fact]
        public void Test_SalleController_GetSalles()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            
            // Création d'une partie et d'une salle de test
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
            var result = controller.GetSalles().Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var salles = Assert.IsAssignableFrom<List<Salle>>(okResult.Value);
            Assert.Single(salles);
            Assert.Equal("Salle test", salles[0].Description);
        }

        [Fact]
        public void Test_AdministrateursController_GetAll()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            var controller = new AdministrateursController(context);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var admins = Assert.IsAssignableFrom<List<Administrateur>>(okResult.Value);
            Assert.Single(admins);
            Assert.Equal("admin", admins[0].NomUtilisateur);
        }

        [Fact]
        public void Test_DonjonsController_GetAll()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            var controller = new DonjonsController(context);

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var donjons = Assert.IsAssignableFrom<List<Donjon>>(okResult.Value);
            Assert.Single(donjons);
            Assert.Equal("Donjon Test", donjons[0].Nom);
        }

        [Fact]
        public void Test_JoueursController_Update()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            var controller = new JoueursController(context);
            var joueurId = context.Joueurs.First().Id;
            var updatedJoueur = new Joueur 
            { 
                Id = joueurId,
                Nom = "JoueurModifié",
                Mail = "modifie@test.com",
                ScoreTotal = 200
            };

            // Act
            var result = controller.Update(joueurId, updatedJoueur);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var joueur = context.Joueurs.Find(joueurId);
            Assert.Equal("JoueurModifié", joueur.Nom);
            Assert.Equal(200, joueur.ScoreTotal);
        }

        [Fact]
        public void Test_JoueursController_Delete()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            var controller = new JoueursController(context);
            var joueurId = context.Joueurs.First().Id;

            // Act
            var result = controller.Delete(joueurId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Joueurs);
        }

        [Fact]
        public void Test_PartieController_CreatePartie()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            SeedTestData(context);
            var controller = new PartieController(context);
            var joueurId = context.Joueurs.First().Id;
            var nouvellePartie = new Partie
            {
                JoueurId = joueurId,
                EstTerminee = false,
                ScoreFinal = 0
            };

            // Act
            var result = controller.PostPartie(nouvellePartie).Result;

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var partie = Assert.IsType<Partie>(createdResult.Value);
            Assert.Equal(joueurId, partie.JoueurId);
            Assert.Single(context.Parties);
        }

        [Fact]
        public void Test_SalleController_ExecuterAction()
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
                Niveau = NiveauDifficulte.Moyen,
                ChoixPossible = new List<ChoixAction> { ChoixAction.Combattre, ChoixAction.Fouiller }
            };
            context.Salles.Add(salle);
            context.SaveChanges();

            var controller = new SalleController(context);

            // Act
            var result = controller.ExecuterAction(salle.Id, ChoixAction.Combattre).Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultat = okResult.Value; // ActionResultat
            Assert.NotNull(resultat);
        }
    }
}