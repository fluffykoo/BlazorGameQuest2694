using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Partie
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid(); // Identifiant unique de la partie

    [ForeignKey(nameof(Joueur))]
    public Guid JoueurId { get; set; } // Référence au joueur
    public Joueur? Joueur { get; set; } // navigation inverse

    [ForeignKey(nameof(Donjon))]
    public Guid DonjonId { get; set; }
    public Donjon? Donjon { get; set; }

    [InverseProperty(nameof(Salle.Partie))]
    public List<Salle> Salles { get; set; } = new(); // Donjon composé de plusieurs salles

    public int ScoreFinal { get; set; } // Score obtenu à la fin de la partie
    public DateTime Date { get; set; } = DateTime.UtcNow; // Date de la partie
    public bool EstTerminee { get; set; } // True si la partie est finie
}