using DaberlyProjet.Data;
using DaberlyProjet.DTO;
using DaberlyProjet.Models;
using DaberlyProjet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProduitController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProduitDTO>>> GetProduits()
        {
            var produits = await _context.Produits
                .Where(p => p.Visible)
                .Select(p => new ProduitDTO
                {
                    Nom = p.Nom,
                    Description = p.Description,
                    Prix = p.Prix,
                    Marque = p.Marque,
                    Visible = p.Visible
                })
                .ToListAsync();

            return Ok(produits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProduitDTO>> GetProduit(int id)
        {
            var produit = await _context.Produits
                .Where(p => p.Id == id)
                .Select(p => new ProduitDTO
                {
                    Nom = p.Nom,
                    Description = p.Description,
                    Prix = p.Prix,
                    Marque = p.Marque,
                    Visible = p.Visible
                })
                .FirstOrDefaultAsync();

            if (produit == null)
            {
                return NotFound();
            }

            return Ok(produit);
        }

        [HttpPost]
        public async Task<ActionResult<Produit>> PostProduit(ProduitDTO produitDTO)
        {
            var produit = new Produit
            {
                Nom = produitDTO.Nom,
                Description = produitDTO.Description,
                Prix = produitDTO.Prix,
                Marque = produitDTO.Marque,
                Visible = false
            };

            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduit), new { id = produit.Id }, produit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduit(int id, ProduitDTO produitDTO)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            produit.Nom = produitDTO.Nom;
            produit.Description = produitDTO.Description;
            produit.Prix = produitDTO.Prix;
            produit.Marque = produitDTO.Marque;
            produit.Visible = produitDTO.Visible;

            _context.Entry(produit).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
