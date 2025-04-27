using DaberlyProjet.Data;
using DaberlyProjet.DTO;
using DaberlyProjet.Models;
using DaberlyProjet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AlbumService _albumService;
        public AlbumController(AppDbContext context , AlbumService albumService)
        {
            _context = context;
            _albumService = albumService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            var albums = await _context.Albums.Include(a => a.Produit).ToListAsync();
            return Ok(albums);
        }

       
        [HttpGet("produit/{produitId}")]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbumsByProduit(int produitId)
        {
            var albums = await _context.Albums.Where(a => a.ProduitId == produitId).ToListAsync();

            if (albums == null || !albums.Any())
            {
                return NotFound();
            }

            return Ok(albums);
        }


        [HttpPost]
        public async Task<ActionResult<Album>> PostAlbum([FromForm] AlbumDTO albumDTO, IFormFile file)
        {
            var produit = await _context.Produits.FindAsync(albumDTO.ProduitId);
            if (produit == null)
            {
                return NotFound("Produit non trouvé");
            }

            if (file == null)
            {
                return BadRequest("Aucun fichier téléchargé");
            }

            var fileType = await DetermineFileType(file);

            if (fileType == "Image")
            {
                albumDTO.EstPhoto = true; 
            }
            else if (fileType == "Vidéo")
            {
                albumDTO.EstPhoto = false; 
            }
            else
            {
                return BadRequest("Type de fichier non pris en charge");
            }

            var album = new Album
            {
                Url = await _albumService.UploadFiles(file),
                EstPhoto = albumDTO.EstPhoto,
                ProduitId = albumDTO.ProduitId
            };

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<string> DetermineFileType(IFormFile file)
        {
            var imageMimeTypes = new List<string> { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            var videoMimeTypes = new List<string> { "video/mp4", "video/avi", "video/mkv", "video/webm" };

            var mimeType = file.ContentType;

            if (imageMimeTypes.Contains(mimeType))
            {
                return "Image";
            }
            else if (videoMimeTypes.Contains(mimeType))
            {
                return "Vidéo";
            }
            else
            {
                return "Type de fichier non pris en charge";
            }
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
