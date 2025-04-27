using DaberlyProjet.Data;
using DaberlyProjet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointureController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PointureController(AppDbContext context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pointure>>> GetPointures()
        {
            var pointures = await _context.Pointures.ToListAsync();
            return Ok(pointures);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Pointure>> GetPointure(int id)
        {
            var pointure = await _context.Pointures.FindAsync(id);

            if (pointure == null)
            {
                return NotFound();
            }

            return Ok(pointure);
        }

        [HttpGet("getByTaille/{taille}")]
        public async Task<IActionResult> getPointureByTaille(string taille)
        {
            var pointure = await _context.Pointures.Where(p => p.Taille == taille).FirstOrDefaultAsync();
            if (pointure == null)
            {
                return NotFound();
            }
            return Ok(pointure);
        }
        
        [HttpPost]
        public async Task<ActionResult<Pointure>> PostPointure(string pointure)
        {
            var x = new Pointure { Taille =  pointure };
            _context.Pointures.Add(x);
            await _context.SaveChangesAsync();

            return Ok();
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPointure(int id, Pointure pointure)
        {
            if (id != pointure.Id)
            {
                return BadRequest();
            }

            _context.Entry(pointure).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePointure(int id)
        {
            var pointure = await _context.Pointures.FindAsync(id);
            if (pointure == null)
            {
                return NotFound();
            }

            _context.Pointures.Remove(pointure);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
