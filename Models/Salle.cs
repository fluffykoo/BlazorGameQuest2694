using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Models;

public enum NiveauDifficulte
{
    Facile,
    Moyen,
    Difficile

}

public enum ChoixAction
{
    Combattre,
    Fuir,
    Fouiller,
}
[Owned]//indique à EF Core que ce n’est pas une entité indépendante
public class ActionResultat
{
    public ChoixAction Action { get; set; }
    public int Points { get; set; }
    public bool EstPiege { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class Salle
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();// Identifiant unique de la salle

    [ForeignKey(nameof(Partie))]
    public Guid PartieId { get; set; }
    public Partie? Partie { get; set; }

    [ForeignKey(nameof(Donjon))]
    public Guid DonjonId { get; set; }
    public Donjon? Donjon { get; set; }
    
    public int Position { get; set; }// Numéro de la salle dans le donjon
    public string Description { get; set; } = string.Empty;// Texte affiché au joueur
    public NiveauDifficulte Niveau { get; set; }// Difficulté de la salle
    public List<ChoixAction> ChoixPossible { get; set; } = new(); // Liste des choix offerts au joueur
    public ChoixAction? ChoixFait { get; set; }// Choix effectué par le joueur
    public ActionResultat? Resultat { get; set; }// Résultat de l’action choisie
}
