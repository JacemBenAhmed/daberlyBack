using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaberlyProjet.Data;
using DaberlyProjet.Models;
using DaberlyProjet.DTO;

namespace DaberlyProjet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProduitPointureCouleurController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProduitPointureCouleurController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProduitPointureCouleur>>> GetAll()
        {
            return await _context.ProduitPointureCouleurs
                .Include(p => p.Produit)
                .Include(p => p.Pointure)
                .Include(p => p.Couleur)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProduitPointureCouleur>> GetById(int id)
        {
            var item = await _context.ProduitPointureCouleurs
                .Include(p => p.Produit)
                .Include(p => p.Pointure)
                .Include(p => p.Couleur)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ProduitPointureCouleur>> Create(ProduitPointureCouleurDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = new ProduitPointureCouleur
            {
                ProduitId = dto.ProduitId,
                PointureId = dto.PointureId,
                CouleurId = dto.CouleurId,
                Quantite = dto.qte
            };

            _context.ProduitPointureCouleurs.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProduitPointureCouleurDTO dto)
        {
            var existingEntity = await _context.ProduitPointureCouleurs.FindAsync(id);
            if (existingEntity == null)
            {
                return NotFound();
            }

            existingEntity.ProduitId = dto.ProduitId;
            existingEntity.PointureId = dto.PointureId;
            existingEntity.CouleurId = dto.CouleurId;
            existingEntity.Quantite = dto.qte;


            _context.Entry(existingEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitPointureCouleurExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ProduitPointureCouleurs.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.ProduitPointureCouleurs.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduitPointureCouleurExists(int id)
        {
            return _context.ProduitPointureCouleurs.Any(e => e.Id == id);
        }
    }
}
