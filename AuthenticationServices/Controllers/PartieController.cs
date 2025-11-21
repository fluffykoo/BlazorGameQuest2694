using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using AuthenticationServices.Data;

namespace AuthenticationServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartieController : ControllerBase
    {
        private readonly AventureDbContext _context;
        private readonly Random _random = new Random();

        public PartieController(AventureDbContext context)
        {
            _context = context;
        }

        // POST: api/Partie/demarrer
        // Amélioration V3 : Génération procédurale du donjon au démarrage
        [HttpPost("demarrer")]
        public async Task<ActionResult<Partie>> DemarrerPartie([FromQuery] Guid joueurId)
        {
            // 1. Création de la partie
            var partie = new Partie
            {
                Id = Guid.NewGuid(),
                JoueurId = joueurId,
                Date = DateTime.UtcNow,
                ScoreFinal = 0,
                EstTerminee = false,
                Salles = new List<Salle>() // On va remplir ça tout de suite
            };

            // 2. Génération de 5 salles aléatoires
            for (int i = 0; i < 5; i++)
            {
                partie.Salles.Add(GenererSalleAleatoire(partie.Id, i + 1));
            }

            _context.Parties.Add(partie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartie", new { id = partie.Id }, partie);
        }

        // Méthode utilitaire privée pour la génération procédurale
        private Salle GenererSalleAleatoire(Guid partieId, int position)
        {
            var typesMonstres = new[] { "Gobelin", "Orc", "Squelette", "Dragonnet" };
            var images = new[] { "goblin.png", "orc.png", "skeleton.png", "dragon.png" };
            
            int index = _random.Next(typesMonstres.Length);
            
            // La difficulté augmente avec la position (Salle 5 est plus dure)
            var difficulte = position switch {
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
                PartieId = partieId,
                Position = position,
                EstVisitee = false,
                NomMonstre = typesMonstres[index],
                ImageMonstre = images[index],
                PvMonstre = 10 * position, // PV croissants
                ForceMonstre = 2 * position,
                Niveau = difficulte,
                Description = $"Une salle sombre numéro {position}. Un {typesMonstres[index]} vous regarde."
            };
        }

        // GET: api/Partie/{id} (Reste inchangé mais inclut les salles triées)
        [HttpGet("{id}")]
        public async Task<ActionResult<Partie>> GetPartie(Guid id)
        {
            var partie = await _context.Parties
                .Include(p => p.Joueur)
                .Include(p => p.Salles) // Important pour le front
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partie == null) return NotFound();
            
            // On s'assure que les salles sont dans l'ordre
            partie.Salles = partie.Salles.OrderBy(s => s.Position).ToList();

            return partie;
        }    

        // GET: api/Partie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partie>>> GetParties()
        {
            return await _context.Parties
                .Include(p => p.Joueur)
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

        // PUT: api/Partie/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartie(Guid id, Partie partie)
        {
            if (id != partie.Id)
            {
                return BadRequest();
            }

            _context.Entry(partie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Partie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartie(Guid id)
        {
            var partie = await _context.Parties.FindAsync(id);
            if (partie == null)
            {
                return NotFound();
            }

            _context.Parties.Remove(partie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Partie/5/terminer
        [HttpPatch("{id}/terminer")]
        public async Task<IActionResult> TerminerPartie(Guid id, [FromBody] int scoreFinal)
        {
            var partie = await _context.Parties.FindAsync(id);
            if (partie == null)
            {
                return NotFound();
            }

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