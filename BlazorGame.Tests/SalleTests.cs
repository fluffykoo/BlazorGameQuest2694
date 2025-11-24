using System;
using System.Collections.Generic;

using BlazorGame.Domain;
using Xunit;

namespace BlazorGame.Tests
{
    public class SalleTests
    {
        [Fact]
        public void Salle_Initialisation_DoitAvoirValeursParDefaut()
        {
            // Arrange & Act
            var salle = new Salle();

            // Assert
            Assert.NotEqual(Guid.Empty, salle.Id);
            Assert.Equal(0, salle.Position);
            Assert.Equal(string.Empty, salle.Description ?? string.Empty);
            Assert.Empty(salle.ChoixPossible);
            Assert.Null(salle.ChoixFait);
            Assert.Null(salle.Resultat);
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
            Assert.Equal(difficulte, salle.Niveau);
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
            Assert.Equal(3, salle.ChoixPossible.Count);
            Assert.Equal(choix, salle.ChoixPossible);
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
            Assert.Equal(choix, salle.ChoixFait);
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
            Assert.NotNull(salle.Resultat);
            Assert.Equal(ChoixAction.Fouiller, salle.Resultat.Action);
            Assert.Equal(25, salle.Resultat.Points);
            Assert.False(salle.Resultat.EstPiege);
            Assert.Equal("Vous avez trouvé un trésor!", salle.Resultat.Message);
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
            Assert.Equal(description, salle.Description);
            Assert.Equal(position, salle.Position);
        }
    }
}