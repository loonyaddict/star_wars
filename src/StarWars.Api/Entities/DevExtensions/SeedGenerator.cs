using System;
using System.Collections.Generic;

namespace StarWars.Api.Entities.DevExtensions
{
    public class SeedGenerator
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
            "Solo", "Coruscant", "Heir", "Crait", "D", "Dagobah",
            "Dathomir", "Devaron", "Tales", "Eadu", "Ruusan", "Toydaria",
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

        public IEnumerable<Character> GetNewSeed(int numberOfCharacters,
            (int min, int max) episodes,
            (int min, int max) friends)
        {
            var unusedNames = new List<string>(names);

            throw new NotImplementedException();
            while (numberOfCharacters > 0)
            {
                var character = new Character
                {
                    Id = new Guid(),
                };
            }
        }
    }
}