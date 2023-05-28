using PokeAPI.Models;
using PokeAPI.Models.Joins;
using Microsoft.EntityFrameworkCore;

namespace PokeAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options: options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>()
                        .HasKey((pc) => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>()
                        .HasOne((p) => p.Pokemon)
                        .WithMany((pc) => pc.PokemonCategories)
                        .HasForeignKey((p) => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
                        .HasOne((p) => p.Category)
                        .WithMany((pc) => pc.PokemonCategories)
                        .HasForeignKey((c) => c.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
                        .HasKey((po) => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                        .HasOne((p) => p.Pokemon)
                        .WithMany((pc) => pc.PokemonOwners)
                        .HasForeignKey((p) => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
                        .HasOne((p) => p.Owner)
                        .WithMany((pc) => pc.PokemonOwners)
                        .HasForeignKey((c) => c.OwnerId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = "server=localhost;user=root;password='';database=pokemon";

            var serverVersion = new MySqlServerVersion(new Version(10, 4, 28));

            optionsBuilder.UseMySql(connectionString: connectionStr, serverVersion: serverVersion);

            base.OnConfiguring(optionsBuilder);
        }
    }
}