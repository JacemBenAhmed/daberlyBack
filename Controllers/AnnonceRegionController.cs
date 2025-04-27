using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaberlyProjet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaberlyProjet.Data;
using DaberlyProjet.DTO;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnonceRegionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnnonceRegionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnonceRegions()
        {
            var annonces = await _context.AnnonceRegions.ToListAsync();
            if(annonces!=null)
            {
                return Ok(annonces);
            }
            return BadRequest("no");
        }

        [HttpGet("getByAnnonceID/{annonceId}")]
        public async Task<IActionResult> GetAnnonceRegionById(int annonceId)
        {
            var annonces = await _context.AnnonceRegions.Where(a => a.AnnonceId == annonceId).ToListAsync();
            if (annonces != null)
            {
                return Ok(annonces);
            }
            return BadRequest("no");
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AnnonceRegion>> GetAnnonceRegion(int id)
        {
            var annonceRegion = await _context.AnnonceRegions.FindAsync(id);

            if (annonceRegion == null)
            {
                return NotFound();
            }

            return annonceRegion;
        }

        [HttpPost]
        public async Task<IActionResult> PostAnnonceRegion([FromBody] AnnonceRegionDTO annonceRegionDTO)
        {
            if (annonceRegionDTO == null || string.IsNullOrEmpty(annonceRegionDTO.Region))
            {
                return BadRequest("Les données fournies sont invalides.");
            }

            var annonceExiste = await _context.Annonces.AnyAsync(a => a.id == annonceRegionDTO.AnnonceId);
            if (!annonceExiste)
            {
                return NotFound("L'annonce spécifiée n'existe pas.");
            }

            var annonceRegion = new AnnonceRegion
            {
                AnnonceId = annonceRegionDTO.AnnonceId,
                Region = annonceRegionDTO.Region
            };

            _context.AnnonceRegions.Add(annonceRegion);
            await _context.SaveChangesAsync();

            return Ok("Annonce ajoutée avec succès.");
        }




        [HttpDelete("{id}/{region}")]
        public async Task<IActionResult> DeleteAnnonceRegion(int id , string region)
        {
            var annonceRegion = await _context.AnnonceRegions.Where(ann=>ann.AnnonceId==id && ann.Region==region).FirstOrDefaultAsync();
            if (annonceRegion == null)
            {
                return NotFound();
            }

            _context.AnnonceRegions.Remove(annonceRegion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}