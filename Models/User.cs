using System.ComponentModel.DataAnnotations;

namespace DaberlyProjet.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Addresse { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string numCin { get; set; }
       

        public bool blocked { get; set; }

        public DateTime DateCreated { get; set; }


    }
}
