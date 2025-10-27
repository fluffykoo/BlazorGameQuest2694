using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Joueur
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();// identifiant unique

    [Required]
    public string Nom { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Mail { get; set; } = string.Empty;

    public int ScoreTotal { get; set; }

    // Relation 1 joueur → N parties
    public List<Partie> Historique { get; set; } = new();//historique des parties

    public DateTime DerniereConnexion { get; set; } = DateTime.UtcNow;

    public bool PeutReprendrePartie { get; set; }//True si une partie est en cours
}