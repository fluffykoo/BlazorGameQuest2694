using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using GameServices.Data;

namespace GameServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalleController : ControllerBase
    {
        private readonly GameDbContext _context;

        public SalleController(GameDbContext context)
        {
            _context = context;
        }

        // GET: api/Salle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salle>>> GetSalles()
        {
            return await _context.Salles
                .Include(s => s.Partie)
                .ToListAsync();
        }

        // GET: api/Salle/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Salle>> GetSalle(Guid id)
        {
            var salle = await _context.Salles
                .Include(s => s.Partie)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salle == null)
            {
                return NotFound();
            }

            return salle;
        }

        // GET: api/Salle/partie/{partieId}
        [HttpGet("partie/{partieId}")]
        public async Task<ActionResult<IEnumerable<Salle>>> GetSallesByPartie(Guid partieId)
        {
            return await _context.Salles
                .Where(s => s.PartieId == partieId)
                .OrderBy(s => s.Position)
                .ToListAsync();
        }

        // POST: api/Salle
        [HttpPost]
        public async Task<ActionResult<Salle>> PostSalle(Salle salle)
        {
            _context.Salles.Add(salle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalle", new { id = salle.Id }, salle);
        }

        // POST: api/Salle/batch
        [HttpPost("batch")]
        public async Task<ActionResult> PostSalles([FromBody] List<Salle> salles)
        {
            _context.Salles.AddRange(salles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Salle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalle(Guid id, Salle salle)
        {
            if (id != salle.Id)
            {
                return BadRequest();
            }

            _context.Entry(salle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalleExists(id))
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

        // PATCH: api/Salle/5/action
        [HttpPatch("{id}/action")]
        public async Task<IActionResult> ExecuterAction(Guid id, [FromBody] ChoixAction action)
        {
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
            {
                return NotFound();
            }

            salle.ChoixFait = action;
            // Ici tu pourras ajouter la logique pour générer le résultat de l'action
            salle.Resultat = new ActionResultat
            {
                Action = action,
                Points = CalculerPoints(action, salle.Niveau),
                EstPiege = false, // À implémenter
                Message = $"Action {action} exécutée avec succès"
            };

            await _context.SaveChangesAsync();

            return Ok(salle.Resultat);
        }

        // DELETE: api/Salle/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalle(Guid id)
        {
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
            {
                return NotFound();
            }

            _context.Salles.Remove(salle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalleExists(Guid id)
        {
            return _context.Salles.Any(e => e.Id == id);
        }

        private int CalculerPoints(ChoixAction action, NiveauDifficulte difficulte)
        {
            // Logique de calcul des points basée sur l'action et la difficulté
            var pointsBase = action switch
            {
                ChoixAction.Combattre => 10,
                ChoixAction.Fouiller => 5,
                ChoixAction.Fuir => 2,
                _ => 0
            };

            var multiplicateur = difficulte switch
            {
                NiveauDifficulte.Facile => 1,
                NiveauDifficulte.Moyen => 2,
                NiveauDifficulte.Difficile => 3,
                _ => 1
            };

            return pointsBase * multiplicateur;
        }
    }
}