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

        public PartieController(AventureDbContext context)
        {
            _context = context;
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

        // GET: api/Partie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Partie>> GetPartie(Guid id)
        {
            var partie = await _context.Parties
                .Include(p => p.Joueur)
                .Include(p => p.Salles)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partie == null)
            {
                return NotFound();
            }

            return partie;
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