using AutoMapper;
using Library.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarWars.Api.Controllers
{
    [Route("api/charactercollections")]
    public class CharacterCollectionsController : Controller
    {
        private readonly IStarWarsRepository repository;
        private readonly ILogger<CharacterCollectionsController> logger;

        private void Save(string exceptionMessage = "")
        {
            if (!repository.Save())
                throw new Exception(exceptionMessage);
        }

        public CharacterCollectionsController(IStarWarsRepository repository, ILogger<CharacterCollectionsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet("({ids})", Name = "GetCharacterCollection")]
        public IActionResult GetAuthorCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();

            var characterEntities = repository.GetCharacters(ids);

            if (ids.Count() != characterEntities.Count())
                return NotFound();

            var authorsToReturn = Mapper.Map<IEnumerable<CharacterDto>>(characterEntities);

            return Ok(authorsToReturn);
        }

        [HttpPost]
        public IActionResult CreateAuthorColletion(
            [FromBody] IEnumerable<CharacterForCreationDto> charactersForCreations)
        {
            if (charactersForCreations == null)
                return BadRequest();

            var characterEntities = Mapper.Map<IEnumerable<Character>>(charactersForCreations);

            foreach (var character in characterEntities)
                repository.AddCharacter(character);

            Save(exceptionMessage: "Creating author collection falied on save.");

            var authorCollectionToReturn = Mapper.Map<IEnumerable<CharacterDto>>(characterEntities);
            var idsAsString = string.Join(",",
                authorCollectionToReturn.Select(author => author.Id));

            return CreatedAtRoute("GetCharacterCollection",
                new { ids = idsAsString },
                authorCollectionToReturn);
        }

    }
}