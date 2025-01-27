using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class ProduitPointureCouleur
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("Produit")]
        public int ProduitId { get; set; }
        
        public Produit Produit { get; set; }

        [ForeignKey("Pointure")]
        public int PointureId { get; set; }
        
        public Pointure Pointure { get; set; }

        [ForeignKey("Couleur")]
        public int CouleurId { get; set; }
       
        public Couleur Couleur { get; set; }

        [Required]
        public int Quantite { get; set; }

        
    }

}
