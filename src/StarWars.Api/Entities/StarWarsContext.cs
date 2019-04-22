using Microsoft.EntityFrameworkCore;
using StarWars.Api.Helpers;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Entities
{
    public class StarWarsContext : DbContext
    {
        public StarWarsContext(DbContextOptions<StarWarsContext> options)
           : base(options) =>
            Database.Migrate();

        public DbSet<Character> Characters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
    }

    public static class StarWarsContextExtensions
    {
        /// <summary>
        /// This method clears database and puts some hard coded characters.
        /// </summary>
        /// <param name="context"></param>
        public static void StartWithFreshData(this StarWarsContext context)
        {
            //Remove all character entries from database.
            context.Characters.RemoveRange(context.Characters);
            context.SaveChanges();

            #region Episodes

            var NewHope = new Episode
            {
                Id = new Guid("f28b46e9-d9e6-42cf-9aeb-30bccae2175f"),
                Name = "New hope"
            };

            var Jedi = new Episode
            {
                Id = new Guid("462a5025-cb5c-4eae-9676-d1291c3f676c"),
                Name = "Return of Jedi"
            };

            var Empire = new Episode
            {
                Id = new Guid("e4422d25 - 93d4 - 4dec - 9a11 - f4172ba4c0c4"),
                Name = "Empire strikes back"
            };

            #endregion Episodes

            #region Characters

            var Luke = new Character
            {
                Id = new Guid("a4e36fcb-a44a-478b-af48-8e44a4cceb6f"),
                Name = "Luke Skywalker",
                Episodes = new List<Episode>() { NewHope, Empire, Jedi }
            };

            var Leia = new Character
            {
                Id = new Guid("5936c1ca-288a-43e4-95ab-ee39ea7d5aec"),
                Name = "Leia Organa",
                Planet = "Aldebaran",
                Episodes = new List<Episode>() { NewHope, Empire, Jedi }
            };

            var HanSolo = new Character
            {
                Id = new Guid("acd68dec-e59b-4507-b166-16591169292a"),
                Name = "HanSolo",
                Episodes = new List<Episode>() { NewHope }
            };

            var Vader = new Character
            {
                Id = new Guid("4243ab01-80c0-46ec-9464-df8fb25ebdd5"),
                Name = "Darth Vader",
                Episodes = new List<Episode>() { NewHope }
            };

            #endregion Characters

            Luke.Friends.AddMany(Leia, HanSolo);
            Leia.Friends.AddMany(Luke, HanSolo);
            HanSolo.Friends.AddMany(Luke, Leia);

            context.Episodes.AddRange(NewHope, Jedi, Empire);
            context.Characters.AddRange(Luke, Leia, HanSolo, Vader);

            context.SaveChanges();
        }
    }
}