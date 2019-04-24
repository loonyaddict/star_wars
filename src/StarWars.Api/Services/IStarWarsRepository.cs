using StarWars.Api.Entities;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Services
{
    public interface IStarWarsRepository
    {
        IEnumerable<Character> Characters { get; }

        Character GetCharacter(Guid characterId);

        IEnumerable<Character> GetCharacters(IEnumerable<Guid> characterIds);

        void AddCharacter(Character character);

        void DeleteCharacter(Character character);

        void UpdateCharacter(Character character);

        bool CharacterExists(Guid characterId);

        void AddEpisodeForCharacter(Guid characterId, Episode episode);

        IEnumerable<Episode> GetEpisodesForCharacter(Guid characterId);

        Episode GetEpisodeForCharacter(Guid characterId, Guid episodeId);

        void DeleteEpisode(Episode episode);

        bool Save();
    }
}