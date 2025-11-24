using System;
using System.Collections.Generic;
using BlazorGame.Domain;
using Xunit;

namespace BlazorGame.Tests
{
    public class PartieTests
    {
        [Fact]
        public void Partie_Initialisation_DoitAvoirValeursParDefaut()
        {
            // Arrange & Act
            var partie = new Partie();

            // Assert
            Assert.NotEqual(Guid.Empty, partie.Id);
            Assert.Equal(Guid.Empty, partie.JoueurId);
            Assert.Empty(partie.Salles);
            Assert.Equal(0, partie.ScoreFinal);

            // Date proche de maintenant (tolérance 2 secondes)
            var diff = DateTime.UtcNow - partie.Date;
            Assert.True(diff.TotalSeconds < 2);

            Assert.False(partie.EstTerminee);
        }

        [Fact]
        public void Partie_AvecSalles_DoitMaintenirCollection()
        {
            // Arrange
            var partie = new Partie();
            var salle1 = new Salle { Position = 1 };
            var salle2 = new Salle { Position = 2 };

            // Act
            partie.Salles.Add(salle1);
            partie.Salles.Add(salle2);

            // Assert
            Assert.Equal(2, partie.Salles.Count);
            Assert.Equal(salle1, partie.Salles[0]);
            Assert.Equal(salle2, partie.Salles[1]);
        }

        [Theory]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(500, true)]
        public void Partie_AvecDifferentesValeurs_DoitMaintenirEtat(int scoreFinal, bool estTerminee)
        {
            // Arrange
            var partie = new Partie();
            var joueurId = Guid.NewGuid();

            // Act
            partie.JoueurId = joueurId;
            partie.ScoreFinal = scoreFinal;
            partie.EstTerminee = estTerminee;

            // Assert
            Assert.Equal(joueurId, partie.JoueurId);
            Assert.Equal(scoreFinal, partie.ScoreFinal);
            Assert.Equal(estTerminee, partie.EstTerminee);
        }

        [Fact]
        public void Partie_DateCustom_DoitEtreConservee()
        {
            // Arrange
            var partie = new Partie();
            var dateCustom = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            // Act
            partie.Date = dateCustom;

            // Assert
            Assert.Equal(dateCustom, partie.Date);
        }
    }
}