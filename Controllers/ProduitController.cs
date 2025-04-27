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
            var produits = await _context.Produits.ToListAsync();

            return Ok(produits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProduitDTO>> GetProduit(int id)
        {
            var produit = await _context.Produits
                .Where(p => p.Id == id)
                .Select(p => new Produit
                {
                    Id = p.Id,
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
        [HttpGet("getByName/{name}")]
        public async Task<ActionResult<ProduitDTO>> GetProductByName(string name)
        {
            var produit = await _context.Produits
                .Where(p => p.Nom == name)
                .FirstAsync();

            if (produit == null)
            {
                return NotFound();
            }

            return Ok(produit);
        }

        [HttpGet("getProductByProdPointCoulID/{id}")]
        public async Task<IActionResult> GetProductByProdPointCoulId(int id)
        {
            var productPointCoul = await _context.ProduitPointureCouleurs
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productPointCoul == null)
            {
                return NotFound($"ProduitPointureCouleur avec l'ID {id} non trouvé.");
            }

            var product = await _context.Produits
                .FirstOrDefaultAsync(pr => pr.Id == productPointCoul.ProduitId);

            if (product == null)
            {
                return NotFound($"Produit avec l'ID {productPointCoul.ProduitId} non trouvé.");
            }

            return Ok(new ProduitDTO
            {
                Nom = product.Nom,
                Marque = product.Marque,
                Prix = product.Prix,
            });
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

            return Ok(produit.Id);
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



        [HttpPut("setVisible/{id}")]
        public async Task<IActionResult> setVisible(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            
            produit.Visible = !produit.Visible;

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
