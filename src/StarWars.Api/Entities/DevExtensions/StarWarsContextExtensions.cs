using System;
using System.Collections.Generic;

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

            #region Characters

            var Luke = new Character
            {
                Id = new Guid("a4e36fcb-a44a-478b-af48-8e44a4cceb6f"),
                Name = "Luke Skywalker",
                Planet = "Tatooin",
                Episodes = new List<Episode>
                {
                    new Episode{Name = "Jedi", Id = new Guid("01d5b167-e65d-4896-8ffd-0f8f1117904d")},
                    new Episode{Name = "NewHope", Id = new Guid("9fe34fd3-e5bc-484f-bd2e-c70d53f84687")}
                }
            };

            var Leia = new Character
            {
                Id = new Guid("5936c1ca-288a-43e4-95ab-ee39ea7d5aec"),
                Name = "Leia Organa",
                Planet = "Aldebaran",
                Episodes = new List<Episode>
                {
                    new Episode{Name = "Jedi", Id = new Guid("38d7c69c-d732-4bf3-91c0-5055b8fc47e2")},
                    new Episode{Name = "NewHope", Id = new Guid("8f190925-359b-4756-85f0-e7ed4a78b9cb")}
                }
            };

            var HanSolo = new Character
            {
                Id = new Guid("acd68dec-e59b-4507-b166-16591169292a"),
                Name = "HanSolo",
                Planet = "Tatooin",
                Episodes = new List<Episode>
                {
                    new Episode{Name = "Jedi", Id = new Guid("2c6493d5-959f-46da-8c4f-a468a26f53a3")},
                    new Episode{Name = "NewHope", Id = new Guid("c43586b4-86ae-4286-99aa-6913d440e70c")}
                }
            };

            var Vader = new Character
            {
                Id = new Guid("4243ab01-80c0-46ec-9464-df8fb25ebdd5"),
                Name = "Darth Vader",
                Planet = "Aldebaran",
                Episodes = new List<Episode>
                {
                    new Episode{Name = "Jedi", Id = new Guid("f90b3078-8b37-40b1-bd3c-e9bc9acbabf9")},
                    new Episode{Name = "NewHope", Id = new Guid("7ac37bb9-9009-4419-8217-ffb7a91b2c1a")},
                    new Episode{Name = "Empire", Id = new Guid("c0661f68-6170-4e15-904c-ea5e87a70e8a")}
                }
            };

            #endregion Characters

            context.Characters.AddRange(Luke, Leia, HanSolo, Vader);
            context.SaveChanges();
        }

        public static IEnumerable<string> GetCharactersAsString(
            this StarWarsContext context)
        {
            foreach (var character in context.Characters)
                yield return character.ToString();
        }
    }
}