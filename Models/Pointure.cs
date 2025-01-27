using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class Pointure
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Taille { get; set; }

        public ICollection<ProduitPointureCouleur> ProduitPointureCouleurs { get; set; }
    }

}
