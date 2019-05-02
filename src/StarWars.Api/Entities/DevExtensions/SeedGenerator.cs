using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StarWars.Api.Entities.DevExtensions
{
    internal class SeedGenerator
    {
        private readonly Random random = new Random();

        private readonly List<string> names = new List<string>()
        {
            "Mon Mothma", "Wicket", "Admiral Ackbar", "Qui-Gon Jinn", "Rey", "Poe Dameron",
            "Finn", "Cassian Andor", "Orson Krennic", "DJ", "Boba Fett", "Darth Maul", "C-3PO",
            "BB-8", "Chewbacca", "Lando Calrissian", "Obi-Wan Kenobi", "R2-D2", "Emperor Palpatine",
            "Yoda", "Princess Leia", "Luke Skywalker", "Kylo Ren", "Han Solo", "Darth Vader"
        };

        private readonly List<string> planets = new List<string>()
        {
            "Abafar", "Ahch", "Akiva", "Alderaan", "Ando", "Anoat",
            "Ryloth", "Tales", "Saleucami", "Savareen", "Solo", "Scarif",
            "Shili", "Starkiller", "Subterrel", "Sullust", "Takodana", "Tatooine",
        };

        private readonly List<string> episodes = new List<string>()
        {
            "A New Hope",
            "The Empire Strikes Back",
            "Return of the Jedi",
            "The Phantom Menace",
            "Attack of the Clones",
            "Revenge of the Sith",
            "The Force Awakens",
            "The Last Jedi",
        };

        private string RandomPlanet() => planets[random.Next(planets.Count - 1)];

        private IEnumerable<Episode> RandomEpisodes(int number)
        {
            number = number < episodes.Count
                ? number
                : episodes.Count;
            var unusedEpisodes = new List<string>(episodes);

            while (number > 0)
            {
                int next = random.Next(unusedEpisodes.Count - 1);
                var episodeName = unusedEpisodes[next];
                unusedEpisodes.RemoveAt(next);
                number--;

                yield return new Episode
                {
                    Id = Guid.NewGuid(),
                    Name = episodeName
                };
            }
        }

        internal IEnumerable<Character> GetNewSeed((int min, int max) episodes)
        {
            IList<Character> charactersList = new List<Character>();
            foreach (var characterName in names)
            {
                var character = new Character
                {
                    Id = Guid.NewGuid(),
                    Name = characterName,
                    Planet = RandomPlanet(),
                    Episodes = RandomEpisodes(random.Next(episodes.min, episodes.max)).ToList()
                };
                charactersList.Add(character);
            }
            return charactersList;
        }

        #region ExampleUse

        internal void ExampleUse()
        {
            SeedGenerator generator = new SeedGenerator();
            var charactersToAdd = generator.GetNewSeed(episodes: (0, 2));

            var charactersAsJson = JsonConvert.SerializeObject(charactersToAdd.ToList());
            var directory = @"c:temp\resource\character_seed\";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText($@"{directory}example_seed.json", charactersAsJson);
        }

        #endregion ExampleUse
    }
}