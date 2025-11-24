using System;
using BlazorGame.Domain;
using Xunit;

namespace BlazorGame.Tests
{
    public class EnumsAndActionResultatTests
    {
        [Fact]
        public void NiveauDifficulte_DoitAvoirValeursCorrectes()
        {
            // Arrange & Act
            var valeurs = Enum.GetValues<NiveauDifficulte>();

            // Assert nombre de valeurs
            Assert.Equal(3, valeurs.Length);

            // Ordre des valeurs
            Assert.Equal(NiveauDifficulte.Facile,  (NiveauDifficulte)valeurs.GetValue(0)!);
            Assert.Equal(NiveauDifficulte.Moyen,   (NiveauDifficulte)valeurs.GetValue(1)!);
            Assert.Equal(NiveauDifficulte.Difficile,(NiveauDifficulte)valeurs.GetValue(2)!);

            // Valeurs numériques
            Assert.Equal(0, (int)NiveauDifficulte.Facile);
            Assert.Equal(1, (int)NiveauDifficulte.Moyen);
            Assert.Equal(2, (int)NiveauDifficulte.Difficile);
        }

        [Fact]
        public void ChoixAction_DoitAvoirValeursCorrectes()
        {
            // Arrange & Act
            var valeurs = Enum.GetValues<ChoixAction>();

            // Assert nombre de valeurs
            Assert.Equal(3, valeurs.Length);

            // Ordre des valeurs
            Assert.Equal(ChoixAction.Combattre, (ChoixAction)valeurs.GetValue(0)!);
            Assert.Equal(ChoixAction.Fuir,      (ChoixAction)valeurs.GetValue(1)!);
            Assert.Equal(ChoixAction.Fouiller,  (ChoixAction)valeurs.GetValue(2)!);

            // Valeurs numériques
            Assert.Equal(0, (int)ChoixAction.Combattre);
            Assert.Equal(1, (int)ChoixAction.Fuir);
            Assert.Equal(2, (int)ChoixAction.Fouiller);
        }

        [Fact]
        public void ActionResultat_Initialisation_DoitAvoirValeursParDefaut()
        {
            // Arrange & Act
            var resultat = new ActionResultat();

            // Assert
            Assert.Equal(0, resultat.Points);
            Assert.False(resultat.EstPiege);
            Assert.True(string.IsNullOrEmpty(resultat.Message));
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
            Assert.Equal(ChoixAction.Combattre, resultat.Action);
            Assert.Equal(100, resultat.Points);
            Assert.True(resultat.EstPiege);
            Assert.Equal("C'était un piège!", resultat.Message);
        }
    }
}