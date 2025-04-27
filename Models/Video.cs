namespace DaberlyProjet.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; } 

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }

}
