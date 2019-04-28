using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace StarWars.Api.Entities
{
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

            

            var seedLocation = Path.Combine(
                Environment.CurrentDirectory,
                @"Entities\DevExtensions",
                "star_wars_seed.json");

            var seedContent = File.ReadAllText(seedLocation);
            var charactersToAdd = JsonConvert.DeserializeObject<List<Character>>(seedContent);

            context.Characters.AddRange(charactersToAdd);
            context.SaveChanges();
        }
    }
}