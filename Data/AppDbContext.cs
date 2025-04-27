using DaberlyProjet.Models;
using DaberlyProjet.Services;
using Microsoft.EntityFrameworkCore;

namespace DaberlyProjet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PhotoUser> PhotosUser { get; set; }

        public DbSet<Produit> Produits { get; set; }
        public DbSet<Pointure> Pointures { get; set; }
        public DbSet<Couleur> Couleurs { get; set; }
        public DbSet<ProduitPointureCouleur> ProduitPointureCouleurs { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Annonce> Annonces { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<AgentVendeur> AgentVendeurs { get; set; }

        public DbSet<AnnonceRegion> AnnonceRegions { get; set; }


        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Video> Videos { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Conversation>Conversations { get; set; }
 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AgentVendeur>()
                .HasKey(av => new { av.AgentId, av.VendeurId }); 
        }







    }


}
