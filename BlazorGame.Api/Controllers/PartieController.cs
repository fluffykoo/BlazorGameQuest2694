using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using BlazorGame.Api.Data;
using BlazorGame.Api.GameConfig;

namespace BlazorGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartieController : ControllerBase
    {
        private readonly AventureDbContext _context;
        private readonly Random _random = new();
        private readonly IReadOnlyList<DonjonTemplate> _donjonTemplates = DonjonTemplates.All;

        public PartieController(AventureDbContext context)
        {
            _context = context;
        }

        // POST: api/Partie/demarrer?joueurId=xxxxx
        // Génère :
        //  - un Donjon
        //  - une Partie liée au Joueur + Donjon
        //  - des Salles procédurales liées Partie + Donjon
        [HttpPost("demarrer")]
        public async Task<ActionResult<Partie>> DemarrerPartie([FromQuery] Guid joueurId)
        {
            // 1. Vérifier que le joueur existe
            var joueur = await _context.Joueurs.FindAsync(joueurId);
            if (joueur == null)
            {
                return BadRequest("Le joueur spécifié n'existe pas.");
            }
/*
            // 2. Créer un donjon aléatoire
            var donjon = new Donjon
            {
                Nom = "Donjon Mystérieux",
                Description = "Généré automatiquement",
                NombreDeSalles = 5
            };
            _context.Donjons.Add(donjon);
            await _context.SaveChangesAsync();*/
            // 2. Choisir un template de donjon aléatoirement
            var template = _donjonTemplates[_random.Next(_donjonTemplates.Count)];

            // Nombre de salles aléatoire dans l’intervalle du template
            var nbSalles = _random.Next(template.MinSalles, template.MaxSalles + 1);

            // 3. Créer le donjon basé sur le template
            var donjon = new Donjon
            {
                Nom = template.Nom,
                Description = template.Description,
                NombreDeSalles = nbSalles
            };

            _context.Donjons.Add(donjon);
            await _context.SaveChangesAsync();

            // 4. Créer la partie (on force l’Id pour référencer depuis les salles)
            var partieId = Guid.NewGuid();
            var partie = new Partie
            {
                Id = partieId,
                JoueurId = joueurId,
                DonjonId = donjon.Id,
                Date = DateTime.UtcNow,
                ScoreFinal = 0,
                EstTerminee = false,
                Salles = new List<Salle>()
            };

            // 4. Générer les salles p
            for (int position = 1; position <= donjon.NombreDeSalles; position++)
            {
                partie.Salles.Add(GenererSalleAleatoire(partieId, donjon.Id, position));
            }

            _context.Parties.Add(partie);
            await _context.SaveChangesAsync();

            // 5. Charger les relations avant de renvoyer au front
            await _context.Entry(partie).Reference(p => p.Donjon).LoadAsync();
            await _context.Entry(partie).Collection(p => p.Salles).LoadAsync();

            // On renvoie la partie complète (utilisée par ton front Blazor)
            return Ok(partie);
        }

        // Méthode utilitaire privée pour la génération d'une salle
        private Salle GenererSalleAleatoire(Guid partieId, Guid donjonId, int position)
        {
            var typesMonstres = new[] { "Gobelin", "Orc", "Squelette", "Dragonnet" };
            var images = new[] { "goblin.png", "orc.png", "skeleton.png", "dragon.png" };

            int index = _random.Next(typesMonstres.Length);

            var difficulte = position switch
            {
                1 => NiveauDifficulte.Facile,
                2 => NiveauDifficulte.Facile,
                3 => NiveauDifficulte.Moyen,
                4 => NiveauDifficulte.Moyen,
                5 => NiveauDifficulte.Difficile,
                _ => NiveauDifficulte.Moyen
            };

            return new Salle
            {
                Id = Guid.NewGuid(),
                PartieId = partieId,    // FK vers Partie
                DonjonId = donjonId,    // FK vers Donjon
                Position = position,
                EstVisitee = false,
                NomMonstre = typesMonstres[index],
                ImageMonstre = images[index],
                PvMonstre = 10 * position,
                ForceMonstre = 2 * position,
                Niveau = difficulte,
                Description = $"Salle {position} : un {typesMonstres[index]} vous attend...",
                ChoixPossible = new List<ChoixAction>
                {
                    ChoixAction.Combattre,
                    ChoixAction.Fouiller,
                    ChoixAction.Fuir
                }
            };
        }

        // GET: api/Partie/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Partie>> GetPartie(Guid id)
        {
            var partie = await _context.Parties
                .Include(p => p.Joueur)
                .Include(p => p.Donjon)
                .Include(p => p.Salles)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partie == null) return NotFound();

            partie.Salles = partie.Salles.OrderBy(s => s.Position).ToList();
            return partie;
        }

        // GET: api/Partie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partie>>> GetParties()
        {
            return await _context.Parties
                .Include(p => p.Joueur)
                .Include(p => p.Donjon)
                .Include(p => p.Salles)
                .ToListAsync();
        }

        // GET: api/Partie/joueur/{joueurId}
        [HttpGet("joueur/{joueurId}")]
        public async Task<ActionResult<IEnumerable<Partie>>> GetPartiesByJoueur(Guid joueurId)
        {
            return await _context.Parties
                .Where(p => p.JoueurId == joueurId)
                .Include(p => p.Salles)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        // POST: api/Partie
        [HttpPost]
        public async Task<ActionResult<Partie>> PostPartie(Partie partie)
        {
            _context.Parties.Add(partie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartie", new { id = partie.Id }, partie);
        }

        // PUT: api/Partie/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartie(Guid id, Partie partie)
        {
            if (id != partie.Id) return BadRequest();

            _context.Entry(partie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartieExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Partie/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartie(Guid id)
        {
            var partie = await _context.Parties.FindAsync(id);
            if (partie == null) return NotFound();

            _context.Parties.Remove(partie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Partie/joueur/{id}/encours (pour sauvegarder les parties encours , non terminée )
        [HttpGet("joueur/{joueurId}/encours")]
        public async Task<ActionResult<Partie>> GetPartieEnCours(Guid joueurId)
        {
            var partie = await _context.Parties
                .Include(p => p.Donjon)
                .Include(p => p.Salles)
                .Where(p => p.JoueurId == joueurId && p.EstTerminee == false)
                .OrderByDescending(p => p.Date)
                .FirstOrDefaultAsync();

            if (partie == null)
                return NotFound();

            return partie;
        }

        // PATCH: api/Partie/{id}/terminer
        [HttpPatch("{id}/terminer")]
        public async Task<IActionResult> TerminerPartie(Guid id, [FromBody] int scoreFinal)
        {
            var partie = await _context.Parties.FindAsync(id);
            if (partie == null) return NotFound();

            partie.EstTerminee = true;
            partie.ScoreFinal = scoreFinal;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PartieExists(Guid id)
        {
            return _context.Parties.Any(e => e.Id == id);
        }
    }
}