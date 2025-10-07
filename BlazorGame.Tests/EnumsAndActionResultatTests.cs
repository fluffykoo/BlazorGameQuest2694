using FluentAssertions;
using Models;

namespace UnitTests.Models
{
    public class EnumsAndActionResultatTests
    {
        [Fact]
        public void NiveauDifficulte_DoitAvoirValeursCorrectes()
        {
            // Arrange & Act
            var valeurs = Enum.GetValues<NiveauDifficulte>();

            // Assert
            valeurs.Should().HaveCount(3);
            valeurs.Should().ContainInOrder(
                NiveauDifficulte.Facile,
                NiveauDifficulte.Moyen, 
                NiveauDifficulte.Difficile
            );
            
            // Vérification des valeurs numériques
            ((int)NiveauDifficulte.Facile).Should().Be(0);
            ((int)NiveauDifficulte.Moyen).Should().Be(1);
            ((int)NiveauDifficulte.Difficile).Should().Be(2);
        }

        [Fact]
        public void ChoixAction_DoitAvoirValeursCorrectes()
        {
            // Arrange & Act
            var valeurs = Enum.GetValues<ChoixAction>();

            // Assert
            valeurs.Should().HaveCount(3);
            valeurs.Should().ContainInOrder(
                ChoixAction.Combattre,
                ChoixAction.Fuir,
                ChoixAction.Fouiller
            );
            
            // Vérification des valeurs numériques
            ((int)ChoixAction.Combattre).Should().Be(0);
            ((int)ChoixAction.Fuir).Should().Be(1);
            ((int)ChoixAction.Fouiller).Should().Be(2);
        }

        [Fact]
        public void ActionResultat_Initialisation_DoitAvoirValeursParDefaut()
        {
            // Arrange & Act
            var resultat = new ActionResultat();

            // Assert
            resultat.Points.Should().Be(0);
            resultat.EstPiege.Should().BeFalse();
            resultat.Message.Should().BeEmpty();
        }

        [Fact]
        public void ActionResultat_AvecValeurs_DoitMaintenirEtat()
        {
            // Arrange & Act
            var resultat = new ActionResultat
            {
                Action = ChoixAction.Combattre,
                Points = 100,
                EstPiege = true,
                Message = "C'était un piège!"
            };

            // Assert
            resultat.Action.Should().Be(ChoixAction.Combattre);
            resultat.Points.Should().Be(100);
            resultat.EstPiege.Should().BeTrue();
            resultat.Message.Should().Be("C'était un piège!");
        }
    }
}