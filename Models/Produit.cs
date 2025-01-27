using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class Produit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Nom { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Prix { get; set; }

        [StringLength(255)]
        public string Marque { get; set; }

        [Required]
        public bool Visible { get; set; }

        public ICollection<ProduitPointureCouleur> ProduitPointureCouleurs { get; set; }

        public ICollection<Album> Albums { get; set; }
    }

}
