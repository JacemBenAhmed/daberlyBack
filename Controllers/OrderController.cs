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

                var x = await _context.ProduitPointureCouleurs.FirstOrDefaultAsync(c => c.Id==cartItem.ProduitPointureCouleurId);

                if (x == null)
                {
                    return BadRequest("produit ne pas trouvé ");
                }
                int qte = x.Quantite;

               

                if (qte < cartItem.Quantity)
                {
                    return BadRequest($"Stock insuffisant pour le produit  (disponible : , demandé : {cartItem.Quantity}).");
                }

                //  - stock from prods 
                x.Quantite -= cartItem.Quantity;
                // _context.ProduitPointureCouleurs.Update(x);
               


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
                    Price = totalAmount  
                }))
            };


            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();

            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();


            return Ok(order.Id);
        }

        [HttpGet("GetOrdersByUser/{userId}")]
        public async Task<IActionResult> GetOrders(int userId)
        {
            var orders = await _context.Orders
                
                .Where(o => o.UserId == userId)
                .ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                return NotFound("Aucune commande trouvée.");
            }

            return Ok(orders);
        }

        
        [HttpGet("GetOrder/{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound("Commande non trouvée.");
            }

            return Ok(order);
        }

        [HttpGet("GetOrderItems/{orderId}")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            var orderItems = await _context.OrderItems.Where(o => o.Id == orderId).ToListAsync();
            if (orderItems == null)
                return NotFound("Aucun"); 
            return Ok(orderItems);
        }
    }
}
