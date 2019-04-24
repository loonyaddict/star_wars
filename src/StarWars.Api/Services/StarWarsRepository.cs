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
            //if (character.Episodes.Any())
            //{
            //    foreach (var episode in character.Episodes)
            //    {
            //        episode.Character = character;
            //        episode.CharacterId = character.Id;
            //        episode.Id = Guid.NewGuid();
            //    }
            //}
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

        public void AddEpisodeForCharacter(Guid characterId, Episode episode)
        {
            var character = GetCharacter(characterId);
            if (character != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (character.Id == Guid.Empty)
                {
                    episode.Id = Guid.NewGuid();
                }
            }
            character.Episodes.Add(episode);
        }

        public IEnumerable<Episode> GetEpisodesForCharacter(Guid characterId) =>
            context.Episodes
            .Where(e => e.CharacterId == characterId)
            .OrderBy(e => e.Name)
            .ToList();

        public Episode GetEpisodeForCharacter(Guid characterId, Guid episodeId) =>
            context.Episodes
            .FirstOrDefault(e => e.CharacterId == characterId);

        public void DeleteEpisode(Episode episode) =>
             context.Episodes
             .Remove(episode);
    }
}