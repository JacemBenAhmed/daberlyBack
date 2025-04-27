using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DaberlyProjet.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
