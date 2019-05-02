using AutoMapper;
using Library.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarWars.Api.Controllers.ControllersHelper.CharacterHelper;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;
using StarWars.API.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StarWars.Api.Controllers
{
    /// <summary>
    /// Controls character collections.
    /// </summary>
    [Route("api/charactercollections")]
    public class CharacterCollectionsController : StarWarsController
    {
        private readonly ILogger<CharacterCollectionsController> logger;
        
        /// <summary>
        /// CharacterCollections Controller.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="propertyMappingService"></param>
        /// <param name="typeHelperService"></param>
        public CharacterCollectionsController(IStarWarsRepository repository,
            ILogger<CharacterCollectionsController> logger,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)

            : base(repository, propertyMappingService, typeHelperService) =>
            this.logger = logger;

        /// <summary>
        /// Get collection of characters providing their ids.
        /// </summary>
        /// <remarks>
        /// Not supported by swagger
        /// example use by browser:
        /// https://localhost:44376/api/charactercollections/(ea024f46-5dd8-44d6-9444-110bcb26691f,0021ffa5-bd77-4df5-b61d-738993b92ab3)
        /// </remarks>
        /// <param name="ids"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Post new collection of characters.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// [
        /// 
        ///     {
        ///			"name": "Wookie",
        ///		    "planet": "Furry land"
        ///		},
        ///		{
        ///			"name": "Jabba",
        ///		    "planet": "Desert"
        ///		}
        ///		
        ///	]
        /// </remarks>
        /// <param name="charactersForCreations"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost]
        public IActionResult CreateAuthorColletion(
            [FromBody] IEnumerable<CharacterForCreationDto> charactersForCreations)
        {
            if (charactersForCreations == null)
                return BadRequest();

            var characterEntities = Mapper.Map<IEnumerable<Character>>(charactersForCreations);

            foreach (var character in characterEntities)
            {
                ModelState.CheckModelForSameNameAndPlanet(character);
                if (!ModelState.IsValid)
                    return new UnprocessableEntityObjectResult(ModelState);
            }

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