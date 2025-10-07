using FluentAssertions;
using Models;

namespace UnitTests.Models
{
    public class PartieTests
    {
        [Fact]
        public void Partie_Initialisation_DoitAvoirValeursParDefaut()
        {
            // Arrange & Act
            var partie = new Partie();

            // Assert
            partie.Id.Should().NotBe(Guid.Empty);
            partie.JoueurId.Should().Be(Guid.Empty);
            partie.Salles.Should().BeEmpty();
            partie.ScoreFinal.Should().Be(0);
            partie.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            partie.EstTerminee.Should().BeFalse();
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
            partie.Salles.Should().HaveCount(2);
            partie.Salles.Should().ContainInOrder(salle1, salle2);
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
            partie.JoueurId.Should().Be(joueurId);
            partie.ScoreFinal.Should().Be(scoreFinal);
            partie.EstTerminee.Should().Be(estTerminee);
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
            partie.Date.Should().Be(dateCustom);
        }
    }
}