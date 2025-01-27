using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public bool EstPhoto { get; set; }

        [ForeignKey("Produit")]  
        public int ProduitId { get; set; }
        public Produit Produit { get; set; }
    }

}
