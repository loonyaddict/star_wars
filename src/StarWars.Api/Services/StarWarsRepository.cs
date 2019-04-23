using StarWars.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarWars.Api.Services
{
    public class StarWarsRepository : IStarWarsRepository
    {
        private readonly StarWarsContext context;

        public StarWarsRepository(StarWarsContext context) =>
            this.context = context;

        public void AddCharacter(Character character)
        {
            character.Id = Guid.NewGuid();

            context.Characters
                .Add(character);
        }

        public bool CharacterExists(Guid characterId) =>
            context.Characters
            .Any(c => c.Id == characterId);

        public void DeleteCharacter(Character character) =>
            context.Characters.Remove(character);

        public Character GetCharacter(Guid characterId) =>
            context.Characters
            .FirstOrDefault(c => c.Id == characterId);

        public IEnumerable<Character> GetCharacters(IEnumerable<Guid> characterIds) =>
            context.Characters
            .Where(c => characterIds.Contains(c.Id))
            .OrderBy(character => character.Name);

        public IEnumerable<Character> Characters =>
            context.Characters.OrderBy(c => c.Name);

        public bool Save()
        {
            return context.SaveChanges() >= 0;
        }

        public void UpdateCharacter(Character character)
        {
            throw new NotImplementedException();
        }
    }
}