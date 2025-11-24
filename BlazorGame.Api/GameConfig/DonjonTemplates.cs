namespace BlazorGame.Api.GameConfig;

// Type qui décrit un modèle de donjon
public record DonjonTemplate(
    string Nom,
    string Description,
    int MinSalles,
    int MaxSalles
);

// Fournisseur centralisé des templates de donjons
internal static class DonjonTemplates
{
    // Liste des donjons "types"
    internal static readonly IReadOnlyList<DonjonTemplate> All = new[]
    {
        new DonjonTemplate(
            Nom: "Donjon du Feu",
            Description: "Un donjon brûlant rempli de pièges et de monstres.",
            MinSalles: 4,
            MaxSalles: 5
        ),
        new DonjonTemplate(
            Nom: "Donjon de Glace",
            Description: "Une forteresse gelée où le froid est aussi mortel que les ennemis.",
            MinSalles: 3,
            MaxSalles: 5
        ),
        new DonjonTemplate(
            Nom: "Donjon des Ombres",
            Description: "Une crypte obscure où rôdent des esprits vengeurs.",
            MinSalles: 5,
            MaxSalles: 5
        ),
        new DonjonTemplate(
            Nom: "Donjon de la Forêt Maudite",
            Description: "Une forêt dense et oppressante où chaque arbre semble vous observer.",
            MinSalles: 4,
            MaxSalles: 5
        ),
        new DonjonTemplate(
            Nom: "Donjon du Temple Ancien",
            Description: "Un ancien temple rempli d’énigmes, de trésors… et de gardiens.",
            MinSalles: 3,
            MaxSalles: 5
        )
    };
}