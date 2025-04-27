using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaberlyProjet.Models
{
    public class AgentVendeur
    {
        [Key, Column(Order = 0)]
        public int AgentId { get; set; }  

        [Key, Column(Order = 1)]
        public int VendeurId { get; set; }  

        public virtual User Agent { get; set; }

        public virtual User Vendeur { get; set; }
    }
}
