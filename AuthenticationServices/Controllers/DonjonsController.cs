using Microsoft.AspNetCore.Mvc;
using AuthenticationServices.Data;
using Models;

namespace AuthenticationServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonjonsController : ControllerBase
    {
        private readonly AventureDbContext _context; // récupération du contexte EF Core pour accéder à la base

        public DonjonsController(AventureDbContext context)
        {
            _context = context;
        }

        // GET : api/donjons : renvoie la liste complète des donjons
        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Donjons.ToList());

        // GET : api/donjons/{id} : renvoie un donjon par son Id
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var donjon = _context.Donjons.Find(id);
            return donjon == null ? NotFound() : Ok(donjon);
        }

        // POST : api/donjons : crée un nouveau donjon
        [HttpPost]
        public IActionResult Create(Donjon donjon)
        {
            _context.Donjons.Add(donjon);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = donjon.Id }, donjon);
        }

        // PUT : api/donjons/{id} : modifie un donjon existant
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Donjon donjon)
        {
            var existing = _context.Donjons.Find(id);
            if (existing == null) return NotFound();

            // mise à jour des champs
            existing.Nom = donjon.Nom;
            existing.Description = donjon.Description;
            existing.NombreDeSalles = donjon.NombreDeSalles;
            _context.SaveChanges(); // on sauvegarde en base
            return NoContent();
        }

        // DELETE : api/donjons/{id} : supprime un donjon
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var donjon = _context.Donjons.Find(id);
            if (donjon == null) return NotFound();

            _context.Donjons.Remove(donjon);
            _context.SaveChanges();
            return NoContent();
        }
    }
}