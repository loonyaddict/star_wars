using StarWars.Api.Entities;
using StarWars.Api.Helpers.Pagination;
using StarWars.API.Helpers;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Services
{
    /// <summary>
    /// Repository for manipulating database context within Api.
    /// </summary>
    public interface IStarWarsRepository
    {
        /// <summary>
        /// Get characters PagedList from repository.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        PagedList<Character> GetCharacters(CharacterResourceParameters parameters);

        /// <summary>
        /// Get single character from repository that matches specified id.
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        Character GetCharacter(Guid characterId);

        /// <summary>
        /// Get characters that match specified ids.
        /// </summary>
        /// <param name="characterIds"></param>
        /// <returns></returns>
        IEnumerable<Character> GetCharacters(IEnumerable<Guid> characterIds);

        /// <summary>
        /// Add new character to repository.
        /// </summary>
        /// <param name="character"></param>
        void AddCharacter(Character character);

        /// <summary>
        /// Delete character from repository.
        /// </summary>
        /// <param name="character"></param>
        void DeleteCharacter(Character character);

        /// <summary>
        /// Update character in repository.
        /// </summary>
        /// <param name="character"></param>
        void UpdateCharacter(Character character);

        /// <summary>
        /// Check if character exists.
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        bool CharacterExists(Guid characterId);

        /// <summary>
        /// Add episode for character.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="episode"></param>
        void AddEpisodeForCharacter(Guid characterId, Episode episode);

        /// <summary>
        /// Get all episodes for specified character.
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        IEnumerable<Episode> GetEpisodesForCharacter(Guid characterId);

        /// <summary>
        /// Get specified episode for specified character.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        Episode GetEpisodeForCharacter(Guid characterId, Guid episodeId);

        /// <summary>
        /// Delete specified episode.
        /// </summary>
        /// <param name="episode"></param>
        void DeleteEpisode(Episode episode);

        /// <summary>
        /// Update specified episode for specified character.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="episode"></param>
        void UpdateEpisodeForCharacter(Guid characterId, Episode episode);

        /// <summary>
        /// Save database. Returns false on fail.
        /// </summary>
        /// <returns></returns>
        bool Save();
    }
}