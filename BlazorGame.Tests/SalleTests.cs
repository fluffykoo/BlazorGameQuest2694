using FluentAssertions;
using Models;

namespace UnitTests.Models
{
    public class SalleTests
    {
        [Fact]
        public void Salle_Initialisation_DoitAvoirValeursParDefaut()
        {
            // Arrange & Act
            var salle = new Salle();

            // Assert
            salle.Id.Should().NotBe(Guid.Empty);
            salle.Position.Should().Be(0);
            salle.Description.Should().BeEmpty();
            salle.ChoixPossible.Should().BeEmpty();
            salle.ChoixFait.Should().BeNull();
            salle.Resultat.Should().BeNull();
        }

        [Theory]
        [InlineData(NiveauDifficulte.Facile)]
        [InlineData(NiveauDifficulte.Moyen)]
        [InlineData(NiveauDifficulte.Difficile)]
        public void Salle_AvecDifficulte_DoitMaintenirValeur(NiveauDifficulte difficulte)
        {
            // Arrange
            var salle = new Salle();

            // Act
            salle.Niveau = difficulte;

            // Assert
            salle.Niveau.Should().Be(difficulte);
        }

        [Fact]
        public void Salle_AvecChoixPossibles_DoitMaintenirCollection()
        {
            // Arrange
            var salle = new Salle();
            var choix = new List<ChoixAction> 
            { 
                ChoixAction.Combattre, 
                ChoixAction.Fuir, 
                ChoixAction.Fouiller 
            };

            // Act
            salle.ChoixPossible = choix;

            // Assert
            salle.ChoixPossible.Should().HaveCount(3);
            salle.ChoixPossible.Should().ContainInOrder(choix);
        }

        [Fact]
        public void Salle_AvecChoixFait_DoitEtreEnregistre()
        {
            // Arrange
            var salle = new Salle();
            var choix = ChoixAction.Combattre;

            // Act
            salle.ChoixFait = choix;

            // Assert
            salle.ChoixFait.Should().Be(choix);
        }

        [Fact]
        public void Salle_AvecResultat_DoitMaintenirEtat()
        {
            // Arrange
            var salle = new Salle();
            var resultat = new ActionResultat
            {
                Action = ChoixAction.Fouiller,
                Points = 25,
                EstPiege = false,
                Message = "Vous avez trouvé un trésor!"
            };

            // Act
            salle.Resultat = resultat;

            // Assert
            salle.Resultat.Should().NotBeNull();
            salle.Resultat.Action.Should().Be(ChoixAction.Fouiller);
            salle.Resultat.Points.Should().Be(25);
            salle.Resultat.EstPiege.Should().BeFalse();
            salle.Resultat.Message.Should().Be("Vous avez trouvé un trésor!");
        }

        [Fact]
        public void Salle_DescriptionEtPosition_DoitEtreCorrect()
        {
            // Arrange
            var salle = new Salle();
            var description = "Une salle sombre avec des torches";
            var position = 5;

            // Act
            salle.Description = description;
            salle.Position = position;

            // Assert
            salle.Description.Should().Be(description);
            salle.Position.Should().Be(position);
        }
    }
}