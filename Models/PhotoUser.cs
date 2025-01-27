using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaberlyProjet.Models
{
    public class PhotoUser
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; } 
        public string FCin { get; set; }  
        public string BCin { get; set; }  

        public User User { get; set; }  
    }
}
