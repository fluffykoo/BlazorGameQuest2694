using BlazorGame.Api.Data;
using BlazorGame.Api.GameConfig;
using BlazorGame.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorGame.Api.Services;

public class DonjonGenerator : IDonjonGenerator
{
    private readonly AventureDbContext _context;
    private readonly Random _random;
    private readonly IReadOnlyList<DonjonTemplate> _templates;

    public DonjonGenerator(
        AventureDbContext context,
        IReadOnlyList<DonjonTemplate>? templates = null,
        Random? random = null)
    {
        _context = context;
        _templates = templates ?? DonjonTemplates.All;
        _random = random ?? Random.Shared;
    }

    public async Task<Partie> DemarrerPartieAsync(Guid joueurId, CancellationToken cancellationToken = default)
    {
        if (joueurId == Guid.Empty)
            throw new ArgumentException("joueurId est requis", nameof(joueurId));

        var joueur = await _context.Joueurs
            .FirstOrDefaultAsync(j => j.Id == joueurId, cancellationToken);

        if (joueur == null)
            throw new InvalidOperationException("Joueur introuvable.");

        var template = _templates[_random.Next(_templates.Count)];
        var nbSalles = _random.Next(template.MinSalles, template.MaxSalles + 1);

        var donjon = new Donjon
        {
            Nom = template.Nom,
            Description = template.Description,
            NombreDeSalles = nbSalles
        };

        var partieId = Guid.NewGuid();
        var partie = new Partie
        {
            Id = partieId,
            JoueurId = joueurId,
            Donjon = donjon,
            DonjonId = donjon.Id,
            Date = DateTime.UtcNow,
            ScoreFinal = 0,
            EstTerminee = false,
            Salles = new List<Salle>()
        };

        var types = ConstruireTypesDeSalle(nbSalles);
        for (int position = 1; position <= nbSalles; position++)
        {
            var isCombat = types[position - 1];
            partie.Salles.Add(GenererSalleAleatoire(partieId, donjon.Id, position, isCombat));
        }

        _context.Parties.Add(partie);
        await _context.SaveChangesAsync(cancellationToken);

        // Charge les relations dans le même contexte (utile pour le front)
        await _context.Entry(partie).Reference(p => p.Donjon).LoadAsync(cancellationToken);
        await _context.Entry(partie).Collection(p => p.Salles).LoadAsync(cancellationToken);
        partie.Salles = partie.Salles.OrderBy(s => s.Position).ToList();

        return partie;
    }

    private List<bool> ConstruireTypesDeSalle(int nbSalles)
    {
        // On force un mix : majoritairement combat, au moins un coffre si on a plus d'une salle
        var combatsVoulus = Math.Max((int)Math.Round(nbSalles * 0.7), 1);
        var coffresVoulus = nbSalles - combatsVoulus;
        if (coffresVoulus == 0 && nbSalles > 1)
        {
            coffresVoulus = 1;
            combatsVoulus = nbSalles - coffresVoulus;
        }

        var types = Enumerable.Repeat(true, combatsVoulus)  // true = combat
            .Concat(Enumerable.Repeat(false, coffresVoulus)) // false = coffre
            .ToList();

        // Shuffle
        for (int i = types.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (types[i], types[j]) = (types[j], types[i]);
        }
        return types;
    }

    private Salle GenererSalleAleatoire(Guid partieId, Guid donjonId, int position, bool isCombat)
    {
        var typesMonstres = new[] { "Gobelin", "Orc", "Squelette", "Dragonnet" };
        var images = new[] { "goblin.png", "orc.png", "skeleton.png", "dragon.png" };

        var difficulte = position switch
        {
            <= 2 => NiveauDifficulte.Facile,
            3 or 4 => NiveauDifficulte.Moyen,
            >= 5 => NiveauDifficulte.Difficile
        };

        if (isCombat)
        {
            int index = _random.Next(typesMonstres.Length);

            return new Salle
            {
                Id = Guid.NewGuid(),
                PartieId = partieId,
                DonjonId = donjonId,
                Position = position,
                EstVisitee = false,
                NomMonstre = typesMonstres[index],
                ImageMonstre = images[index],
                PvMonstre = 8 + (position * 4),
                ForceMonstre = 1 + (position * 2),
                Niveau = difficulte,
                Description = $"Salle {position} : un {typesMonstres[index]} vous attend...",
                ChoixPossible = new List<ChoixAction>
                {
                    ChoixAction.Combattre,
                    ChoixAction.Fuir
                }
            };
        }

        return new Salle
        {
            Id = Guid.NewGuid(),
            PartieId = partieId,
            DonjonId = donjonId,
            Position = position,
            EstVisitee = false,
            NomMonstre = "Coffre mystérieux",
            ImageMonstre = "chest.png",
            PvMonstre = 0,
            ForceMonstre = 0,
            Niveau = difficulte,
            Description = $"Salle {position} : un coffre poussiéreux vous intrigue...",
            ChoixPossible = new List<ChoixAction>
            {
                ChoixAction.Fouiller,
                ChoixAction.Fuir
            }
        };
    }
}
