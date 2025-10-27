using Microsoft.AspNetCore.Mvc;
using AuthenticationServices.Data;
using Models;

namespace AuthenticationServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministrateursController : ControllerBase
    {
        private readonly AventureDbContext _context; // récupération du contexte EF Core pour accéder à la base

        public AdministrateursController(AventureDbContext context)
        {
            _context = context;
        }

        // GET : api/administrateurs : renvoie la liste complète des administrateurs
        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Administrateurs.ToList());

        // GET : api/administrateurs/{id} : renvoie un administrateur par son Id
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var admin = _context.Administrateurs.Find(id);
            return admin == null ? NotFound() : Ok(admin);
        }

        // POST : api/administrateurs : crée un nouvel administrateur
        [HttpPost]
        public IActionResult Create(Administrateur admin)
        {
            _context.Administrateurs.Add(admin);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = admin.Id }, admin);
        }

        // PUT : api/administrateurs/{id} : modifie un administrateur existant
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Administrateur admin)
        {
            var existing = _context.Administrateurs.Find(id);
            if (existing == null) return NotFound();

            // mise à jour des champs
            existing.NomUtilisateur = admin.NomUtilisateur;
            existing.Email = admin.Email;
            existing.MotDePasse = admin.MotDePasse;
            _context.SaveChanges(); // on sauvegarde en base
            return NoContent();
        }

        // DELETE : api/administrateurs/{id} : supprime un administrateur
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var admin = _context.Administrateurs.Find(id);
            if (admin == null) return NotFound();

            _context.Administrateurs.Remove(admin);
            _context.SaveChanges();
            return NoContent();
        }
    }
}