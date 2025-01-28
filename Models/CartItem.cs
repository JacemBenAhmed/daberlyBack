using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaberlyProjet.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [Required]
        [ForeignKey("ProduitPointureCouleur")]
        public int ProduitPointureCouleurId { get; set; }
        public ProduitPointureCouleur ProduitPointureCouleur { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
