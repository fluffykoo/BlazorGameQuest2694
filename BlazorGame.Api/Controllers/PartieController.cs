using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorGame.Domain;
using BlazorGame.Api.Data;
using BlazorGame.Api.Contracts;
using BlazorGame.Api.Services;

namespace BlazorGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartieController : ControllerBase
    {
        private readonly AventureDbContext _context;
        private readonly IDonjonGenerator _donjonGenerator;

       public PartieController(AventureDbContext context, IDonjonGenerator donjonGenerator)
        {
            _context = context;
            _donjonGenerator = donjonGenerator;
        }

        // POST: api/Partie/demarrer?joueurId=xxxxx
        // Génère :
        //  - un Donjon
        //  - une Partie liée au Joueur + Donjon
        //  - des Salles procédurales liées Partie + Donjon
        [HttpPost("demarrer")]
        public async Task<ActionResult<Partie>> DemarrerPartie([FromQuery] Guid joueurId, CancellationToken cancellationToken)
        {
            if (joueurId == Guid.Empty)
                return BadRequest("joueurId est requis.");

            try
            {
                var partieCreee = await _donjonGenerator.DemarrerPartieAsync(joueurId, cancellationToken);
                return Ok(partieCreee);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la génération du donjon : {ex.Message}");
            }
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
        public async Task<IActionResult> TerminerPartie(Guid id, [FromBody] TerminerPartieRequest request)
        {
            if (!ModelState.IsValid || request == null)
                return BadRequest(ModelState);

            var partie = await _context.Parties.FindAsync(id);
            if (partie == null) return NotFound();

            partie.EstTerminee = true;
            partie.ScoreFinal = request.ScoreFinal;

            await RecalculerScoreJoueur(partie.JoueurId);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PartieExists(Guid id)
        {
            return _context.Parties.Any(e => e.Id == id);
        }

        private async Task RecalculerScoreJoueur(Guid joueurId)
        {
            var joueur = await _context.Joueurs.FindAsync(joueurId);
            if (joueur == null) return;

            var total = await _context.Parties
                .Where(p => p.JoueurId == joueurId && p.EstTerminee)
                .SumAsync(p => (int?)p.ScoreFinal) ?? 0;

            joueur.ScoreTotal = total;
        }
    }
}
