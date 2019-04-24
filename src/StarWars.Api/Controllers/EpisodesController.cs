using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Controllers
{
    [Route("api/characters/{characterId}/episodes")]
    public class EpisodesController : Controller
    {
        private readonly IStarWarsRepository repository;

        public EpisodesController(IStarWarsRepository repository) =>
            this.repository = repository;

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

            var episodeEntity = Mapper.Map<Episode>(episode);

            repository.AddEpisodeForCharacter(characterId, episodeEntity);

            if (!repository.Save())
                throw new Exception($"Adding Episode for Character {characterId} falied on save.");

            var episodeToReturn = Mapper.Map<EpisodeDto>(episodeEntity);

            return CreatedAtRoute("GetEpisodeForCharacter",
                new
                {
                    characterId,
                    episodeId = episodeToReturn.Id
                },
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

            if (!repository.Save())
                throw new Exception($"Deleting Episode {episodeId} for author {characterId} falied on save.");

            return NoContent();
        }
    }
}