using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;
using StarWars.API.Services;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Controllers
{
    /// <summary>
    /// Support various operations on episodes in repository.
    /// </summary>
    [Route("api/characters/{characterId}/episodes")]
    public class EpisodesController : StarWarsController
    {
        private readonly ILogger<EpisodesController> logger;

        /// <summary>
        /// Episodes Controller.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="propertyMappingService"></param>
        /// <param name="typeHelperService"></param>
        public EpisodesController(IStarWarsRepository repository,
            ILogger<EpisodesController> logger,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)

            : base(repository, propertyMappingService, typeHelperService) =>
            this.logger = logger;




        /// <summary>
        /// Gets episodes for specified character.
        /// </summary>
        /// <remarks>
        /// Some characters ids from seed:
        /// b1ffc6ab-3c09-4fc0-a21d-6c7445753219
        /// 2de7652d-b826-4dd3-ba76-57feb2154fc6
        /// 199a49bc-6e0c-4906-8ab9-f321ffc8e764
        /// ea024f46-5dd8-44d6-9444-110bcb26691f
        /// ebbd1f7d-269b-4312-973a-ecbe2eebc223
        /// </remarks>
        /// <param name="characterId"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet]
        public IActionResult GetEpisodesForCharacter(Guid characterId)
        {
            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodesFromRepo = repository.GetEpisodesForCharacter(characterId);
            var episodesForCharacter = Mapper.Map<IEnumerable<EpisodeDto>>(episodesFromRepo);

            return Ok(episodesForCharacter);
        }


        /// <summary>
        /// Gets specified episode for specified character
        /// </summary>
        /// <remarks>
        /// Existing character id:
        /// 557e7ac2-28e7-466e-8d34-10b7d72ea66d
        /// Existing episode id for that character:
        /// ca3979cb-aa6a-457a-9b66-170335925959
        /// </remarks>
        /// <param name="characterId"></param>
        /// <param name="episodeId"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{episodeId}", Name = "GetEpisodeForCharacter")]
        public IActionResult GetEpisodeForCharacter(Guid characterId, Guid episodeId)
        {
            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodeFromRepo = repository.GetEpisodeForCharacter(characterId, episodeId);

            var episodeForCharacter = Mapper.Map<EpisodeDto>(episodeFromRepo);

            return Ok(episodeForCharacter);
        }

        /// <summary>
        /// Add episode to specified character. 
        /// </summary>
        /// <remarks>
        /// Existing character id:
        /// 1d26bfac-aa69-406b-abfd-6d1990929632
        /// Sample request:
        ///
        ///     {
        ///        "name": "Some Mouse Company Adaptation",
        ///     }
        ///
        /// </remarks>
        /// <param name="characterId"></param>
        /// <param name="episode">EpisodeForCreationDto</param>
        /// <returns>Newly created character</returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult AddEpisodeForCharacter(Guid characterId,
            [FromBody] EpisodeForCreationDto episode)
        {
            if (episode == null)
                return BadRequest();

            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodeEntity = Mapper.Map<Episode>(episode);

            repository.AddEpisodeForCharacter(characterId, episodeEntity);

            Save(exceptionMessage: $"Adding Episode for Character {characterId} falied on save.");

            var episodeToReturn = Mapper.Map<EpisodeDto>(episodeEntity);

            return CreatedAtRoute("GetEpisodeForCharacter",
                new { characterId, episodeId = episodeToReturn.Id },
                episodeToReturn);
        }


        /// <summary>
        /// Deletes specified episode for specified character from repository
        /// </summary>
        /// <remarks>
        /// Existing character id:
        /// 1b4d8cda-2da3-403c-a4c0-c835b5861d29
        /// Existing episode id for that character:
        /// 4f11a283-cc51-4499-84ae-ef3da7899742
        /// </remarks>
        /// <param name="characterId"></param>
        /// /// <param name="episodeId"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpDelete("{episodeId}")]
        public IActionResult DeleteEpisodeForCharacter(Guid characterId, Guid episodeId)
        {
            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodeFromRepo = repository.GetEpisodeForCharacter(characterId, episodeId);

            if (episodeFromRepo == null)
                return NotFound();

            repository.DeleteEpisode(episodeFromRepo);

            Save(exceptionMessage: $"Deleting Episode {episodeId} for character {characterId} falied on save.");

            logger.LogInformation(100, $"Episode {episodeId} for character {characterId} was deleted.");

            return NoContent();
        }

        /// <summary>
        /// Update specified episode for specified character with EpisodeForUpdateDto model.
        /// Will override previous episode instance in repository.
        /// Will create new episode for character if episodeId does not exist in repository.
        /// </summary>
        /// <remarks>
        /// Existing character id: 
        /// e751e291-34bf-4fb4-811b-24e1b81fb1e8
        /// Sample request:
        ///
        ///     {
        ///        "name": "Newest hope",
        ///     }
        ///
        /// </remarks>
        /// <param name="characterId"></param>
        /// <param name="episodeId"></param>
        /// <param name="episode">EpisodeForUpdateDto</param>
        /// <returns></returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpPut("{episodeId}")]
        public IActionResult UpdateEpisodeForCharacter(
            Guid characterId, Guid episodeId,
            [FromBody] EpisodeForUpdateDto episode)
        {
            if (episode == null)
                return BadRequest();

            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodeFromRepo = repository.GetEpisodeForCharacter(characterId, episodeId);

            if (episodeFromRepo == null)
                return UpsertEpisodeFromPut(characterId, episodeId, episode);

            Mapper.Map(episode, episodeFromRepo);
            repository.UpdateEpisodeForCharacter(characterId, episodeFromRepo);

            Save(exceptionMessage: $"PUT episode {episodeId} for character {characterId} falied on save.");

            return NoContent();
        }

        private IActionResult UpsertEpisodeFromPut(
            Guid characterId, Guid episodeId,
            EpisodeForUpdateDto episode)
        {
            var episodeToAdd = Mapper.Map<Episode>(episode);
            episodeToAdd.Id = episodeId;

            repository.AddEpisodeForCharacter(characterId, episodeToAdd);

            Save(exceptionMessage: $"Upserting episode {episodeId} for character {characterId} falied on save.");

            var episodeToReturn = Mapper.Map<EpisodeDto>(episodeToAdd);

            return CreatedAtRoute("GetEpisodeForCharacter",
                new { characterId, episodeId = episodeToReturn.Id },
                episodeToReturn);
        }


        /// <summary>
        /// PartiallyUpdate specified episode for specified character with json patch.
        /// Supports upsertion.
        /// </summary>
        /// <remarks>
        /// Existing character id: 
        /// 9d5e38d7-900d-4b7d-a9b9-20c0f5059d61
        /// Existing episode id for that character:
        /// 93dc2261-8882-4193-bae9-a628f6b1728b
        /// Requests must be placed in [ ] array.
        /// Sample request:
        ///[
        ///
        ///     {
        ///         "op": "replace",
        ///         "path": "/name",
        ///         "value": "Empire Strikes Again"
        ///     },
        ///
        /// ]
        /// </remarks>
        /// <param name="characterId"></param>
        /// <param name="episodeId"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [HttpPatch("{episodeId}")]
        public IActionResult PartiallyUpdateEpisodeForCharacter(
            Guid characterId, Guid episodeId,
            [FromBody] JsonPatchDocument<EpisodeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodeFromRepo = repository.GetEpisodeForCharacter(characterId, episodeId);

            if (episodeFromRepo == null)
                return UpsertEpisodeFromPatch(characterId, episodeId, patchDoc);

            var episodeToPatch = Mapper.Map<EpisodeForUpdateDto>(episodeFromRepo);

            patchDoc.ApplyTo(episodeToPatch, ModelState);
            TryValidateModel(episodeToPatch);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            Mapper.Map(episodeToPatch, episodeFromRepo);

            Save(exceptionMessage: $"PATCH episode {episodeId} for character {characterId} falied on save.");

            return NoContent();
        }

        private IActionResult UpsertEpisodeFromPatch(
            Guid characterId, Guid episodeId,
            JsonPatchDocument<EpisodeForUpdateDto> patchDoc)
        {
            var episodeDto = new EpisodeForUpdateDto();

            patchDoc.ApplyTo(episodeDto, ModelState);
            TryValidateModel(episodeDto);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var episodeToAdd = Mapper.Map<Episode>(episodeDto);

            repository.AddEpisodeForCharacter(characterId, episodeToAdd);

            Save(exceptionMessage: $"Upserting episode {episodeId} for character {characterId} falied on save.");

            var episodeToReturn = Mapper.Map<EpisodeDto>(episodeToAdd);

            return CreatedAtRoute("GetEpisodeForCharacter",
            new { characterId, episodeId = episodeToReturn.Id },
            episodeToReturn);
        }
    }
}