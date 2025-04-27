using DaberlyProjet.Models;
using DaberlyProjet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaberlyProjet.DTO;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentVendeurController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgentVendeurController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAgentVendeurs()
        {
            var agents= await _context.AgentVendeurs.ToListAsync();
            return Ok(agents);
        }

        [HttpGet("getAgentsVendeursID/{id}")]
        public async Task<IActionResult> GetAgentVendeurByVendeur(int id)
        {
            var agentVendeurs = await _context.AgentVendeurs
                .Where(av => av.AgentId == id)
                .Include(av => av.Vendeur)
                .ToListAsync();

            if (agentVendeurs == null || !agentVendeurs.Any())
            {
                return NotFound();
            }

            var vendeurs = agentVendeurs.Select(av => av.Vendeur).ToList();

            return Ok(vendeurs);
        }

        [HttpPost]
        public async Task<IActionResult> PostAgentVendeur(AgentVendeursDTO agentVendeurDto)
        {
            var agent = await _context.Users.FindAsync(agentVendeurDto.AgentId);
            var vendeur = await _context.Users.FindAsync(agentVendeurDto.VendeurId);

            if (agent == null || vendeur == null)
            {
                return BadRequest("Agent or Vendeur does not exist.");
            }

            var agentVendeur = new AgentVendeur
            {
                AgentId = agentVendeurDto.AgentId,
                VendeurId = agentVendeurDto.VendeurId
            };

            _context.AgentVendeurs.Add(agentVendeur);
            await _context.SaveChangesAsync();

            return Ok();
        }





        [HttpDelete]
        public async Task<IActionResult> DeleteAgentVendeur(int idAgent , int idVendeur)
        {
            var agentVendeur = await _context.AgentVendeurs.Where(u=>u.VendeurId==idVendeur && u.AgentId==idAgent).FirstAsync();
            if (agentVendeur == null)
            {
                return NotFound();
            }

            _context.AgentVendeurs.Remove(agentVendeur);
            await _context.SaveChangesAsync();

            return Ok();
        }

       
    }
}
