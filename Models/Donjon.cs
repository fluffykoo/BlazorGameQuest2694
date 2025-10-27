using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Donjon
{
    // clé primaire unique générée automatiquement
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // nom du donjon (champ obligatoire)
    [Required]
    public string Nom { get; set; } = string.Empty;

    // description du donjon (par défaut : texte générique)
    public string Description { get; set; } = "Un mystérieux donjon inconnu...";

    // nombre de salles que contient le donjon
    public int NombreDeSalles { get; set; }

    // relation : un donjon possède plusieurs salles
    [InverseProperty(nameof(Salle.Donjon))]
    public List<Salle> Salles { get; set; } = new();

    // relation : un donjon peut être associé à plusieurs parties
    [InverseProperty(nameof(Partie.Donjon))]
    public List<Partie> Parties { get; set; } = new();
}