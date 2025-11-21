using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using AuthenticationServices.Data;

namespace AuthenticationServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalleController : ControllerBase
    {
        private readonly AventureDbContext _context;

        public SalleController(AventureDbContext context)
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

        // PATCH: api/Salle/{id}/action
        [HttpPatch("{id}/action")]
        public async Task<ActionResult<ActionResultat>> ExecuterAction(Guid id, [FromBody] ChoixAction action)
        {
            var salle = await _context.Salles.Include(s => s.Partie).FirstOrDefaultAsync(s => s.Id == id);
            if (salle == null || salle.Partie == null) return NotFound();

            if (salle.ChoixFait != null) return BadRequest("Action déjà effectuée dans cette salle.");

            var resultat = new ActionResultat { Action = action };
            var random = new Random();

            // Logique de jeu V3
            switch (action)
            {
                case ChoixAction.Combattre:
                    bool victoire = random.Next(100) > 30; // 70% de chance
                    if (victoire)
                    {
                        resultat.Points = 50 * (int)salle.Niveau;
                        resultat.Message = $"Victoire ! Vous avez terrassé le {salle.NomMonstre}.";
                        salle.PvMonstre = 0;
                    }
                    else
                    {
                        resultat.Points = 0;
                        resultat.Message = $"Échec... Le {salle.NomMonstre} vous a blessé.";
                        // Perte de PV (Simulée ici, idéalement ajouter un champ PV sur Joueur ou Partie)
                    }
                    break;

                case ChoixAction.Fuir:
                    resultat.Points = 10;
                    resultat.Message = "Vous avez fui lâchement mais vous êtes en vie.";
                    break;

                case ChoixAction.Fouiller:
                    bool tresor = random.Next(100) > 50;
                    if (tresor)
                    {
                        resultat.Points = 30;
                        resultat.Message = "Vous avez trouvé une potion rare !";
                    }
                    else
                    {
                        resultat.Points = -10;
                        resultat.EstPiege = true;
                        resultat.Message = "C'était un piège ! Vous perdez des PV.";
                    }
                    break;
            }

            // Mise à jour de l'état
            salle.ChoixFait = action;
            salle.Resultat = resultat;
            salle.EstVisitee = true;
            
            // Mise à jour du score global de la partie
            salle.Partie.ScoreFinal += resultat.Points;
            
            // Vérifier si c'était la dernière salle (Position 5)
            if (salle.Position >= 5)
            {
                salle.Partie.EstTerminee = true;
                resultat.Message += " (FIN DU DONJON)";
            }

            await _context.SaveChangesAsync();

            return Ok(resultat);
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