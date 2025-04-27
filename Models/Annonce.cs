using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class Annonce
    {
        [Key]
        public int id {  get; set; }
        public string text { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
