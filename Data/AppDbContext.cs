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
        public DbSet<Album> Albums { get; set; }

        public static implicit operator AppDbContext(AlbumService v)
        {
            throw new NotImplementedException();
        }
    }

   
}
