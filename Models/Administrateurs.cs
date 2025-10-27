using System.ComponentModel.DataAnnotations;

namespace Models;

public class Administrateur
{
    // clé primaire unique générée automatiquement
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    // nom d'utilisateur de l'administrateur (champ obligatoire)
    [Required]
    public string NomUtilisateur { get; set; } = string.Empty;

    // adresse mail (obligatoire + validation du format email)
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // mot de passe de l'administrateur (champ obligatoire)
    [Required]
    public string MotDePasse { get; set; } = string.Empty;

    // liste optionnelle des joueurs gérées par cet administrateur
    public List<Joueur>? JoueursSupervises { get; set; }
}