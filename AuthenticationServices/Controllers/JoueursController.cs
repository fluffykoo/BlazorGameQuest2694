using Microsoft.AspNetCore.Mvc;
using AuthenticationServices.Data;
using Models;

[ApiController]
[Route("api/[controller]")]
public class JoueursController : ControllerBase
{
    private readonly AventureDbContext _context;//récupération du contexte EF Core pour accéder à la base

    public JoueursController(AventureDbContext context)
    {
        _context = context;
    }

    //GET : api/joueurs: renvoie la liste complète des joueurs
    [HttpGet]
    public IActionResult GetAll() => Ok(_context.Joueurs.ToList());

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
        existing.ScoreTotal = joueur.ScoreTotal;
        _context.SaveChanges();// on sauvegarde en base
        return NoContent();
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