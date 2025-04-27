using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using DaberlyProjet.Models;
using System.Linq;
using System.Threading.Tasks;
using DaberlyProjet.Data;
using DaberlyProjet.DTO;
using Microsoft.EntityFrameworkCore;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnonceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<AnnonceHub> _hubContext;

        public AnnonceController(AppDbContext context, IHubContext<AnnonceHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var annonces = _context.Annonces.ToList();
            return Ok(annonces);
        }

        [HttpGet("getAnnonceById")]
        public async Task<IActionResult> GetAnnonceById(int id)
        {
            var annonce = await _context.Annonces.Where(u => u.id == id).FirstAsync();
            if(annonce == null)
            {
                return NotFound("not found");
            }

            return Ok(annonce);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnnonceDTO newAnnonce)
        {
            if (newAnnonce == null) return BadRequest("Annonce data is required.");

            var annonce = new Annonce
            {
                DateCreated = DateTime.UtcNow,
                text = newAnnonce.text
            };

            _context.Annonces.Add(annonce);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("AnnonceCreated", annonce.id);

            return Ok(annonce.id);
        }

        [HttpPut("modifAnnonce")]
        public async Task<IActionResult> modifAnnonce(int id, string txt)
        {
            var annonce = await _context.Annonces.FindAsync(id);
            if (annonce == null)
            {
                return NotFound("not found");
            }

            annonce.text = txt;
            await _context.SaveChangesAsync();


            await _hubContext.Clients.All.SendAsync("AnnonceUpdated", id);

            return Ok(annonce);
        }

        [HttpDelete("deleteAnnonce")]
        public async Task<IActionResult> deleteAnnonce(int id)
        {
            var annonce = await _context.Annonces.FindAsync(id);
            if (annonce == null)
            {
                return NotFound();
            }

            _context.Annonces.Remove(annonce);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("AnnonceDeleted",id);

            return Ok();
        }



    }
}
