using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class Couleur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        public ICollection<ProduitPointureCouleur> ProduitPointureCouleurs { get; set; }
    }

}
