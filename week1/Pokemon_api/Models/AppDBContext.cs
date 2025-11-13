using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace API_1.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        { 
        }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<Trainer> Trainer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1:N
            modelBuilder.Entity<Pokemon>()
                .HasOne(p => p.Trainer)
                .WithMany(t => t.Pokemons)
                .HasForeignKey(p => p.OwnerID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade); // ลบ Trainer แล้ว Pokémon หายด้วย
        }
    }
}
