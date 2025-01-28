using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaberlyProjet.Models;
using DaberlyProjet.DTO;
using System.Linq;
using System.Threading.Tasks;
using DaberlyProjet.Data;

namespace DaberlyProjet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.ProduitPointureCouleur)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems.Count == 0)
            {
                return BadRequest("Le panier est vide ou n'existe pas.");
            }

            decimal totalAmount = 0;
            foreach (var cartItem in cart.CartItems)
            {
                var product = cartItem.ProduitPointureCouleur; 

                var p = await _context.Produits.FirstOrDefaultAsync(c => c.Id == product.ProduitId);

                totalAmount += cartItem.Quantity * p.Prix; 
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderItems = await Task.WhenAll(cart.CartItems.Select(async ci => new OrderItem
                {
                    ProduitPointureCouleurId = ci.ProduitPointureCouleurId,
                    Quantity = ci.Quantity,
                    Price = await _context.Produits
                            .Where(p => p.Id == ci.ProduitPointureCouleur.ProduitId)
                            .Select(p => p.Prix)
                            .FirstOrDefaultAsync()  
                }))
            };


            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetOrders/{userId}")]
        public async Task<IActionResult> GetOrders(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProduitPointureCouleur)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                return NotFound("Aucune commande trouvée.");
            }

            return Ok(orders);
        }

        // Obtenir une commande par ID
        [HttpGet("GetOrder/{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProduitPointureCouleur)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound("Commande non trouvée.");
            }

            return Ok(order);
        }
    }
}
