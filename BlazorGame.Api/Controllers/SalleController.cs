using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorGame.Domain;
using BlazorGame.Api.Data;

namespace BlazorGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalleController : ControllerBase
    {
        private readonly AventureDbContext _context;
        private readonly Random _random = new();

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

        // GET: api/Salle/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Salle>> GetSalle(Guid id)
        {
            var salle = await _context.Salles
                .Include(s => s.Partie)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salle == null)
                return NotFound();

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

            return CreatedAtAction(nameof(GetSalle), new { id = salle.Id }, salle);
        }

        // POST: api/Salle/batch
        [HttpPost("batch")]
        public async Task<ActionResult> PostSalles([FromBody] List<Salle> salles)
        {
            _context.Salles.AddRange(salles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Salle/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalle(Guid id, Salle salle)
        {
            if (id != salle.Id)
                return BadRequest();

            _context.Entry(salle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalleExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // PATCH: api/Salle/{id}/action
        [HttpPatch("{id}/action")]
        public async Task<ActionResult<ActionResultat>> ExecuterAction(Guid id, [FromBody] ChoixAction action)
        {
            var salle = await _context.Salles
                .Include(s => s.Partie)
                .ThenInclude(p => p!.Salles)
                .Include(s => s.Partie)!.ThenInclude(p => p!.Joueur)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salle == null || salle.Partie == null)
                return NotFound();

            if (salle.ChoixFait != null)
                return BadRequest("Action déjà effectuée dans cette salle.");

            var resultat = new ActionResultat { Action = action };

            bool isCombat = salle.ChoixPossible.Contains(ChoixAction.Combattre);
            bool isCoffre = !isCombat && salle.ChoixPossible.Contains(ChoixAction.Fouiller);

            // Logique de jeu V3
            switch (action)
            {
                case ChoixAction.Combattre:
                    resultat.Risque = 30;
                    bool victoire = _random.Next(100) >= resultat.Risque;
                    if (victoire)
                    {
                        // base combat > 0 même en facile
                        var baseGain = 30 + (20 * (int)salle.Niveau);
                        resultat.Points = baseGain;
                        resultat.Message = $"Victoire ! Vous avez terrassé le {salle.NomMonstre}.";
                        salle.PvMonstre = 0;
                    }
                    else
                    {
                        resultat.Points = -15;
                        resultat.Message = $"Échec... Le {salle.NomMonstre} vous a blessé.";
                    }
                    break;

                case ChoixAction.Fuir:
                    resultat.Risque = 0;
                    resultat.Points = isCombat ? 5 : 0;
                    resultat.Message = isCombat
                        ? "Vous avez fui prudemment et restez en vie."
                        : "Vous laissez le coffre derrière vous.";
                    break;

                case ChoixAction.Fouiller:
                    resultat.Risque = isCoffre ? 50 : 50;
                    bool tresor = _random.Next(100) >= resultat.Risque;

                    if (tresor)
                    {
                        resultat.Points = isCoffre ? 40 : 25;
                        resultat.Message = isCoffre
                            ? "Le coffre renferme un trésor !"
                            : "Vous avez trouvé une potion rare !";
                    }
                    else
                    {
                        resultat.Points = isCoffre ? -20 : -10;
                        resultat.EstPiege = true;
                        resultat.Message = isCoffre
                            ? "C'était un piège ! Vous perdez des points."
                            : "C'était un piège ! Vous perdez des PV.";
                    }
                    break;
            }

            // Mise à jour de l'état de la salle
            salle.ChoixFait = action;
            salle.Resultat = resultat;
            salle.EstVisitee = true;

            // Mise à jour du score global
            salle.Partie.ScoreFinal += resultat.Points;
            resultat.ScoreTotal = salle.Partie.ScoreFinal;

            if (salle.Partie.ScoreFinal < 0)
            {
                salle.Partie.EstTerminee = true;
                resultat.Message += " (Vous êtes mort)";
                await RecalculerScoreJoueur(salle.Partie.JoueurId);
            }
            else
            {
                // Détection de la dernière salle du donjon
                var maxPosition = salle.Partie.Salles?.Max(s => s.Position) ?? salle.Position;
                if (salle.Position >= maxPosition)
                {
                    salle.Partie.EstTerminee = true;
                    resultat.Message += " (FIN DU DONJON)";
                    await RecalculerScoreJoueur(salle.Partie.JoueurId);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(resultat);
        }

        // DELETE: api/Salle/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalle(Guid id)
        {
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null)
                return NotFound();

            _context.Salles.Remove(salle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalleExists(Guid id)
        {
            return _context.Salles.Any(s => s.Id == id);
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
