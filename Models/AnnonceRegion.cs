using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class AnnonceRegion
    {
        [Key]
        public int Id {  get; set; }

        public int AnnonceId { get; set; }

        public string Region { get; set; }

    }
}
