using DaberlyProjet.Data;
using DaberlyProjet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouleurController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CouleurController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Couleur>>> GetCouleurs()
        {
            return await _context.Couleurs.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Couleur>> GetCouleur(int id)
        {
            var couleur = await _context.Couleurs.FindAsync(id);

            if (couleur == null)
            {
                return NotFound();
            }

            return couleur;
        }


        [HttpPost]
        public async Task<ActionResult<Couleur>> PostCouleur( string nom)
        {
            var x = new Couleur{Nom = nom};
            _context.Couleurs.Add(x);
            await _context.SaveChangesAsync();

            return Ok();
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCouleur(int id, string couleur)
        {


            if (string.IsNullOrWhiteSpace(couleur))
            {
                return BadRequest("La couleur ne peut pas être vide.");
            }

            var existingCouleur = await _context.Couleurs.FindAsync(id);
            if (existingCouleur == null)
            {
                return NotFound($"Aucune couleur trouvée avec l'ID {id}.");
            }

            existingCouleur.Nom = couleur;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erreur lors de la mise à jour de la couleur.");
            }

            return NoContent();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCouleur(int id)
        {
            var couleur = await _context.Couleurs.FindAsync(id);
            if (couleur == null)
            {
                return NotFound();
            }

            _context.Couleurs.Remove(couleur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CouleurExists(int id)
        {
            return _context.Couleurs.Any(e => e.Id == id);
        }
    }
}
