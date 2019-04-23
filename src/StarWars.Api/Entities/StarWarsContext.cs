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

            builder.Entity<Character>().ToTable("Characters");
            builder.Entity<Character>().HasKey(p => p.Id);
            builder.Entity<Character>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Character>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Character>().Property(p => p.Planet).HasMaxLength(50);

            builder.Entity<Episode>().ToTable("Episodes");
            builder.Entity<Episode>().Property(p => p.Id);
            builder.Entity<Episode>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Episode>().Property(p => p.Name).IsRequired().HasMaxLength(50);
        }
    }
}