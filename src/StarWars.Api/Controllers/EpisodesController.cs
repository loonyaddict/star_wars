using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Controllers
{
    [Route("api/characters/{characterId}/episodes")]
    public class EpisodesController : StarWarsController
    {
        private readonly ILogger<CharacterController> logger;

        public EpisodesController(IStarWarsRepository repository, ILogger<CharacterController> logger)
            : base(repository) =>
            this.logger = logger;

        [HttpGet]
        public IActionResult GetEpisodesForCharacter(Guid characterId)
        {
            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodesFromRepo = repository.GetEpisodesForCharacter(characterId);
            var episodesForCharacter = Mapper.Map<IEnumerable<EpisodeDto>>(episodesFromRepo);

            return Ok(episodesForCharacter);
        }

        [HttpGet("{episodeId}", Name = "GetEpisodeForCharacter")]
        public IActionResult GetEpisodeForCharacter(Guid characterId, Guid episodeId)
        {
            if (!repository.CharacterExists(characterId))
                return NotFound();

            var episodeFromRepo = repository.GetEpisodeForCharacter(characterId, episodeId);

            var episodeForCharacter = Mapper.Map<EpisodeDto>(episodeFromRepo);

            return Ok(episodeForCharacter);
        }

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