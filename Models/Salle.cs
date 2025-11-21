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

[Owned]
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
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(Partie))]
    public Guid PartieId { get; set; }
    public Partie? Partie { get; set; }

    // Rendu nullable (?) car dans la V3 générée procéduralement, 
    // la salle appartient à la Partie, pas forcément à un "Donjon" prédéfini.
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