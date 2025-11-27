using Microsoft.AspNetCore.Mvc;
using BlazorGame.Api.Data;
using BlazorGame.Domain;

namespace BlazorGame.Api.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    public class JoueursController : ControllerBase
    {
        private readonly AventureDbContext _context;//récupération du contexte EF Core pour accéder à la base

        public JoueursController(AventureDbContext context)
        {
            _context = context;
        }

        //GET : api/joueurs: renvoie la liste des joueurs actifs
        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Joueurs.Where(j => j.EstActif).ToList());

        [HttpGet("classement")]
        public IActionResult GetClassement()
        {
            var classement = _context.Parties
                .Where(p => p.EstTerminee)
                .GroupBy(p => p.JoueurId)
                .Select(g => new
                {
                    JoueurId = g.Key,
                    Score = g.Sum(p => p.ScoreFinal),
                    Parties = g.Count()
                })
                .Join(_context.Joueurs,
                    g => g.JoueurId,
                    j => j.Id,
                    (g, j) => new
                    {
                        j.Id,
                        j.Nom,
                        j.Mail,
                        j.EstActif,
                        ScoreTotal = g.Score,
                        PartiesJouees = g.Parties,
                        j.DerniereConnexion
                    })
                .Where(x => x.EstActif)
                .OrderByDescending(x => x.ScoreTotal)
                .ThenBy(x => x.Nom)
                .ToList();

            return Ok(classement);
        }

        // Classement complet pour l'admin (actifs et inactifs)
        [HttpGet("classement-admin")]
        public IActionResult GetClassementAdmin()
        {
            var classement = _context.Parties
                .Where(p => p.EstTerminee)
                .GroupBy(p => p.JoueurId)
                .Select(g => new
                {
                    JoueurId = g.Key,
                    Score = g.Sum(p => p.ScoreFinal),
                    Parties = g.Count()
                })
                .Join(_context.Joueurs,
                    g => g.JoueurId,
                    j => j.Id,
                    (g, j) => new
                    {
                        j.Id,
                        j.Nom,
                        j.Mail,
                        j.EstActif,
                        ScoreTotal = g.Score,
                        PartiesTerminees = g.Parties,
                        j.DerniereConnexion
                    })
                .OrderByDescending(x => x.ScoreTotal)
                .ThenBy(x => x.Nom)
                .ToList();

            return Ok(classement);
        }

        [HttpGet("export")]
        public IActionResult ExportJson()
        {
            var stats = _context.Parties
                .Where(p => p.EstTerminee)
                .GroupBy(p => p.JoueurId)
                .Select(g => new
                {
                    JoueurId = g.Key,
                    Score = g.Sum(p => p.ScoreFinal),
                    Parties = g.Count()
                })
                .ToDictionary(x => x.JoueurId, x => x);

            var payload = _context.Joueurs
                .ToList() // matérialise pour pouvoir utiliser TryGetValue
                .Select(j =>
                {
                    stats.TryGetValue(j.Id, out var info);
                    var score = info?.Score ?? 0;
                    var parties = info?.Parties ?? 0;
                    return new
                    {
                        j.Nom,
                        j.Mail,
                        ScoreTotal = score,
                        PartiesJouees = parties,
                        j.EstActif
                    };
                })
                .ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json; charset=utf-8", "joueurs.json");
        }

        // GET : api/joueurs/{id} : renvoie un joueur par son Id
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var joueur = _context.Joueurs.Find(id);
            return joueur == null ? NotFound() : Ok(joueur);
        }

        // POST : api/joueurs : crée un nouveau joueur
        [HttpPost]
        public IActionResult Create(Joueur joueur)
        {
            _context.Joueurs.Add(joueur);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = joueur.Id }, joueur);
        }

        // PUT : api/joueurs/{id} : modifie un joueur existant
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Joueur joueur)
        {
            var existing = _context.Joueurs.Find(id);
            if (existing == null) return NotFound();

            // mise à jours des champs
            existing.Nom = joueur.Nom;
            existing.Mail = joueur.Mail;
            existing.EstActif = joueur.EstActif;
            _context.SaveChanges();// on sauvegarde en base
            return NoContent();
        }

        // PATCH : api/joueurs/{id}/toggle : active/désactive un joueur
        [HttpPatch("{id}/toggle")]
        public IActionResult Toggle(Guid id)
        {
            var joueur = _context.Joueurs.Find(id);
            if (joueur == null) return NotFound();

            joueur.EstActif = !joueur.EstActif;
            _context.SaveChanges();
            return Ok(new { joueur.Id, joueur.EstActif });
        }

        // DELETE : api/joueurs/{id} : supprime un joueur
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var joueur = _context.Joueurs.Find(id);
            if (joueur == null) return NotFound();

            _context.Joueurs.Remove(joueur);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
