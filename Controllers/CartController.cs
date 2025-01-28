using Microsoft.AspNetCore.Mvc;
using DaberlyProjet.Models;
using DaberlyProjet.DTO;
using System.Linq;
using DaberlyProjet.Data;
using Microsoft.EntityFrameworkCore;

namespace DaberlyProjet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int userId, CartItemDTO cartItemDTO)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                await _context.Carts.AddAsync(cart);
            }

            var existingCartItem = cart.CartItems
                .FirstOrDefault(ci => ci.CartId == cart.Id);

            if (existingCartItem == null)
            {
                var cartItem = new CartItem
                {
                    ProduitPointureCouleurId = cartItemDTO.ProduitPointureCouleurId,
                    Quantity = 0
                };
                cart.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            var response = new
            {
                cart.Id,
                cart.UserId,
                CartItems = cart.CartItems.Select(ci => new
                {
                    ci.Id,
                    ci.ProduitPointureCouleurId,
                    ci.Quantity
                })
            };

            return Ok(response);
        }




        [HttpGet("GetCartByUser/{userId}")]
        public IActionResult GetCartByUser(int userId)
        {
            var cart = _context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    CartItems = c.CartItems.Select(ci => new
                    {
                        ci.ProduitPointureCouleurId,
                        ci.Quantity,
                        Produit = new
                        {
                            ci.ProduitPointureCouleur.Produit.Id,
                            ci.ProduitPointureCouleur.Produit.Nom
                        }
                    })
                })
                .FirstOrDefault();

            if (cart == null)
                return NotFound("Cart not found for this user.");

            return Ok(cart);
        }

        [HttpPut("UpdateCartItem/{cartItemId}")]
        public IActionResult UpdateCartItem(int cartItemId, [FromBody] CartItemDTO cartItemDTO)
        {
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
                return NotFound("CartItem not found.");

            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.ProduitPointureCouleurId = cartItemDTO.ProduitPointureCouleurId;

            _context.SaveChanges();
            return Ok(cartItem);
        }

        [HttpDelete("RemoveFromCart/{cartItemId}")]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
                return NotFound("CartItem not found.");

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
            return Ok("CartItem removed successfully.");
        }
    }
}
