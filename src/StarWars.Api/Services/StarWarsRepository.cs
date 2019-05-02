using Library.API.Helpers;
using StarWars.Api.Entities;
using StarWars.Api.Helpers;
using StarWars.Api.Helpers.Pagination;
using StarWars.Api.Models;
using StarWars.API.Helpers;
using StarWars.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarWars.Api.Services
{
    /// <summary>
    /// StarWars.Api repository
    /// </summary>
    public class StarWarsRepository : IStarWarsRepository
    {
        private readonly StarWarsContext context;

        private readonly IPropertyMappingService propertyMappingService;

        /// <summary>
        /// Manipulation of database context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertyMappingService"></param>
        public StarWarsRepository(StarWarsContext context,
            IPropertyMappingService propertyMappingService)
        {
            this.context = context;
            this.propertyMappingService = propertyMappingService;
        }

        /// <summary>
        /// Add new character to repository.
        /// </summary>
        /// <param name="character"></param>
        public void AddCharacter(Character character)
        {
            // if id was not set (ex: upsert) we should generate one
            if (character.Id == Guid.Empty)
                character.Id = Guid.NewGuid();

            context.Characters
                .Add(character);

            if (character.Episodes.Any())
            {
                foreach (var episode in character.Episodes)
                    episode.Id = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Check if character exists.
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public bool CharacterExists(Guid characterId) =>
            context.Characters
            .Any(c => c.Id == characterId);

        /// <summary>
        /// Delete character from repository.
        /// </summary>
        /// <param name="character"></param>
        public void DeleteCharacter(Character character) =>
            context.Characters.Remove(character);

        /// <summary>
        /// Get single character from repository that matches specified id.
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public Character GetCharacter(Guid characterId) =>
            context.Characters
            .FirstOrDefault(c => c.Id == characterId);

        /// <summary>
        /// Get characters that match specified ids.
        /// </summary>
        /// <param name="characterIds"></param>
        /// <returns></returns>
        public IEnumerable<Character> GetCharacters(IEnumerable<Guid> characterIds) =>
            context.Characters
            .Where(c => characterIds.Contains(c.Id))
            .OrderBy(character => character.Name);

        /// <summary>
        /// Get characters PagedList from repository.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public PagedList<Character> GetCharacters(CharacterResourceParameters parameters)
        {
            var collectionBeforePaging = context.Characters
                .ApplySort(parameters.OrderBy,
                propertyMappingService.GetPropertyMapping<CharacterDto, Character>());

            if (!string.IsNullOrEmpty(parameters.Planet))
            {
                var planetQuery = StringHelpers.TrimToLowerInvariant(parameters.Planet);

                collectionBeforePaging = collectionBeforePaging
                    .Where(c => StringHelpers.TrimToLowerInvariant(c.Planet) == planetQuery);
            }

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                var searchQuery = StringHelpers.TrimToLowerInvariant(parameters.SearchQuery);

                collectionBeforePaging = collectionBeforePaging
                    .Where(c =>
                    StringHelpers.TrimToLowerInvariant(c.Name).Contains(searchQuery)
                    || StringHelpers.TrimToLowerInvariant(c.Planet).Contains(searchQuery));
            }

            return PagedList<Character>.Create(collectionBeforePaging,
                parameters.PageNumber,
                parameters.PageSize);
        }

        /// <summary>
        /// Save database. Returns false on fail.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return context.SaveChanges() >= 0;
        }

        /// <summary>
        /// Update character in repository. No implementation needed.
        /// </summary>
        /// <param name="character"></param>
        public void UpdateCharacter(Character character)
        {
        }

        /// <summary>
        /// Add episode for character.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="episode"></param>
        public void AddEpisodeForCharacter(Guid characterId, Episode episode)
        {
            var character = GetCharacter(characterId);

            if (character == null) return;

            // if id was not set (ex: upsert) we should generate one
            if (character.Id == Guid.Empty)
            {
                episode.Id = Guid.NewGuid();
            }
            character.Episodes.Add(episode);
        }

        /// <summary>
        /// Get all episodes for specified character.
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        public IEnumerable<Episode> GetEpisodesForCharacter(Guid characterId) =>
            context.Episodes
            .Where(e => e.CharacterId == characterId)
            .OrderBy(e => e.Name)
            .ToList();

        /// <summary>
        /// Get specified episode for specified character.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        public Episode GetEpisodeForCharacter(Guid characterId, Guid episodeId) =>
            context.Episodes
            .Where(e => e.CharacterId == characterId)
            .FirstOrDefault(e => e.Id == episodeId);

        /// <summary>
        /// Delete specified episode.
        /// </summary>
        /// <param name="episode"></param>
        public void DeleteEpisode(Episode episode) =>
             context.Episodes
             .Remove(episode);

        /// <summary>
        /// Update specified episode for specified character. No implmentation needed.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="episode"></param>
        public void UpdateEpisodeForCharacter(Guid characterId, Episode episode)
        {
        }
    }
}