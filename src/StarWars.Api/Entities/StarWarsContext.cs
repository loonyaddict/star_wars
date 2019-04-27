using Microsoft.EntityFrameworkCore;

namespace StarWars.Api.Entities
{
    public class StarWarsContext : DbContext
    {
        public StarWarsContext(DbContextOptions<StarWarsContext> options)
           : base(options)
        { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Character>()
                .HasMany(c => c.Episodes)
                .WithOne(e => e.Character)
                .HasForeignKey(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}