using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BlazorGame.Domain;

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

[Owned]
public class ActionResultat
{
    public ChoixAction Action { get; set; }

    // Δ de score pour cette action (peut être > 0, 0 ou < 0)
    public int Points { get; set; }

    public bool EstPiege { get; set; }
    public string Message { get; set; } = string.Empty;

    // Pour l'affichage du style "Risque : 30%"
    public int Risque { get; set; }

    // Score global de la partie après cette action
    public int ScoreTotal { get; set; }
}

public class Salle
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(Partie))]
    public Guid PartieId { get; set; }
    public Partie? Partie { get; set; }

    // la salle appartient à la Partie.
    [ForeignKey(nameof(Donjon))]
    public Guid? DonjonId { get; set; } 
    public Donjon? Donjon { get; set; }
    
    public int Position { get; set; }
    public string Description { get; set; } = string.Empty;
    public NiveauDifficulte Niveau { get; set; }
    
    public bool EstVisitee { get; set; } = false; // Pour savoir si le joueur est déjà passé
    
    // Infos du Monstre généré
    public string? NomMonstre { get; set; }
    public string? ImageMonstre { get; set; } // ex: "goblin.png"
    public int PvMonstre { get; set; }
    public int ForceMonstre { get; set; }

    public List<ChoixAction> ChoixPossible { get; set; } = new();
    public ChoixAction? ChoixFait { get; set; }
    public ActionResultat? Resultat { get; set; }
}