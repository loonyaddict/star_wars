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

        }
    }
}