using Microsoft.EntityFrameworkCore;

namespace StarWars.Api.Entities
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class StarWarsContext : DbContext
    {
        /// <summary>
        /// Database context.
        /// </summary>
        /// <param name="options"></param>
        public StarWarsContext(DbContextOptions<StarWarsContext> options)
           : base(options)
        { }

        /// <summary>
        /// Characters
        /// </summary>
        public DbSet<Character> Characters { get; set; }

        /// <summary>
        /// Episodes
        /// </summary>
        public DbSet<Episode> Episodes { get; set; }

        /// <summary>
        /// Add relations and rules to database.
        /// </summary>
        /// <param name="builder"></param>
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