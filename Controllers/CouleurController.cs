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
        public async Task<ActionResult<Couleur>> PostCouleur(string nom)
        {
            var x = new Couleur{Nom = nom};
            _context.Couleurs.Add(x);
            await _context.SaveChangesAsync();

            return Ok();
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCouleur(int id, Couleur couleur)
        {
            if (id != couleur.Id)
            {
                return BadRequest();
            }

            _context.Entry(couleur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouleurExists(id))
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
