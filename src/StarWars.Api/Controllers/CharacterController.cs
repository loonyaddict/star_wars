using AutoMapper;
using Library.API.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarWars.Api.Controllers.ControllersHelper.CharacterHelper;
using StarWars.Api.Entities;
using StarWars.Api.Helpers.Pagination;
using StarWars.Api.Models;
using StarWars.Api.Services;
using StarWars.API.Services;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Controllers
{
    /// <summary>
    /// Support various operations on characters in repository.
    /// </summary>
    [Route("api/characters")]
    public class CharacterController : StarWarsController
    {
        private readonly ILogger<CharacterController> logger;
        private readonly IUrlHelper urlHelper;

        /// <summary>
        /// Character Controller.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="urlHelper"></param>
        /// <param name="propertyMappingService"></param>
        /// <param name="typeHelperService"></param>
        public CharacterController(IStarWarsRepository repository,
            ILogger<CharacterController> logger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)

            : base(repository, propertyMappingService, typeHelperService)
        {
            this.logger = logger;
            this.urlHelper = urlHelper;
        }

        /// <summary>
        /// Gets single character record specified by id.
        /// </summary>
        /// <remarks>
        /// Some characters ids from seed:
        /// b1ffc6ab-3c09-4fc0-a21d-6c7445753219
        /// 2de7652d-b826-4dd3-ba76-57feb2154fc6
        /// 199a49bc-6e0c-4906-8ab9-f321ffc8e764
        /// ea024f46-5dd8-44d6-9444-110bcb26691f
        /// ebbd1f7d-269b-4312-973a-ecbe2eebc223
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCharacter")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetAuthor(Guid id)
        {
            var characterFromRepo = repository.GetCharacter(id);
            if (characterFromRepo == null)
                return NotFound();

            var authorToReturn = Mapper.Map<CharacterDto>(characterFromRepo);

            return Ok(characterFromRepo);
        }

        /// <summary>
        /// Gets characters from repository
        /// </summary>
        /// <remarks>
        /// If result does not fit in one page check total count, page number
        /// and links to previous and next page in response headers.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet(Name = "GetCharacters")]
        public IActionResult GetCharacters(CharacterResourceParameters parameters)

        {
            if (!propertyMappingService.ValidMappingExistsFor<CharacterDto, Character>
               (parameters.OrderBy))
                return BadRequest();

            if (!typeHelperService.TypeHasProperties<CharacterDto>(parameters.Fields))
                return BadRequest();

            var charactersFromRepo = repository.GetCharacters(parameters);

            var previousPageLink = charactersFromRepo.HasPrevious
            ? CreateCharactersResourcesUri(parameters, ResourceUriType.PreviousPage)
            : null;

            var nextPageLink = charactersFromRepo.HasNext
            ? CreateCharactersResourcesUri(parameters, ResourceUriType.NextPage)
            : null;

            var paginationMetadata = new
            {
                totalCount = charactersFromRepo.TotalCount,
                pageSize = charactersFromRepo.PageSize,
                currentPage = charactersFromRepo.CurrentPage,
                totalPages = charactersFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var charactersToReturn = Mapper.Map<IEnumerable<CharacterDto>>(charactersFromRepo);

            return Ok(charactersToReturn.ShapeData(parameters.Fields));
        }

        private string CreateCharactersResourcesUri(
            CharacterResourceParameters parameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetCharacters", new
                    {
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        planet = parameters.Planet,
                        searchQuery = parameters.SearchQuery,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize
                    });

                case ResourceUriType.NextPage:
                    return Url.Link("GetCharacters", new
                    {
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        planet = parameters.Planet,
                        searchQuery = parameters.SearchQuery,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize
                    });

                default:
                    return Url.Link("GetCharacters", new
                    {
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        planet = parameters.Planet,
                        searchQuery = parameters.SearchQuery,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize
                    });
            }
        }

        /// <summary>
        /// Creates new chracter from CharacterToCreate Model
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "name": "Jabba Hut",
        ///        "planet": "Unknown"
        ///     }
        ///
        /// </remarks>
        /// <param name="characterToCreate"></param>
        /// <returns>Newly created character</returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [HttpPost]
        public IActionResult CreateCharacter([FromBody] CharacterForCreationDto characterToCreate)
        {
            if (characterToCreate == null)
                return BadRequest();
            ModelState.CheckModelForSameNameAndPlanet(characterToCreate);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var character = Mapper.Map<Character>(characterToCreate);

            repository.AddCharacter(character);

            Save(exceptionMessage: "Creating character failed on save.");

            var characterToReturn = Mapper.Map<CharacterDto>(character);

            return CreatedAtRoute("GetCharacter",
                new { id = character.Id },
                characterToReturn);
        }

        /// <summary>
        /// Deletes character from repository
        /// </summary>
        /// <remarks>
        /// Some characters ids from seed:
        /// b1ffc6ab-3c09-4fc0-a21d-6c7445753219
        /// 2de7652d-b826-4dd3-ba76-57feb2154fc6
        /// 199a49bc-6e0c-4906-8ab9-f321ffc8e764
        /// ea024f46-5dd8-44d6-9444-110bcb26691f
        /// ebbd1f7d-269b-4312-973a-ecbe2eebc223
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public IActionResult DeleteCharacter(Guid id)
        {
            var characterFromRepo = repository.GetCharacter(id);

            if (characterFromRepo == null)
                return NotFound();

            repository.DeleteCharacter(characterFromRepo);

            Save(exceptionMessage: $"Deleting character {id} falied on save.");

            logger.LogInformation(100, $"Character {id} was deleted.");

            return NoContent();
        }

        /// <summary>
        /// Update specified character with CharacterForUpdateDto model.
        /// Will override previous character instance in repository.
        /// Will create new character if id does not exist in repository.
        /// </summary>
        /// <remarks>
        /// Existing character id: e751e291-34bf-4fb4-811b-24e1b81fb1e8
        /// Sample request:
        ///
        ///     {
        ///        "name": "Wookie"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [HttpPut("{id}")]
        public IActionResult UpdateCharacter(Guid id,
            [FromBody] CharacterForUpdateDto character)
        {
            if (character == null)
                return BadRequest();

            var characterFromRepo = repository.GetCharacter(id);

            if (characterFromRepo == null)
                return UpsertCharacterFromPut(id, character);

            Mapper.Map(character, characterFromRepo);

            ModelState.CheckModelForSameNameAndPlanet(characterFromRepo);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            repository.UpdateCharacter(characterFromRepo);

            Save(exceptionMessage: $"PUT character {id} falied on save.");

            return NoContent();
        }

        private IActionResult UpsertCharacterFromPut(Guid id, CharacterForUpdateDto character)
        {
            var characterToAdd = Mapper.Map<Character>(character);
            characterToAdd.Id = id;

            ModelState.CheckModelForSameNameAndPlanet(characterToAdd);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            repository.AddCharacter(characterToAdd);

            Save(exceptionMessage: $"Upserting character {characterToAdd.Id} failed on save.");

            var characterToReturn = Mapper.Map<CharacterDto>(characterToAdd);

            return CreatedAtRoute("GetCharacter",
            new { id = characterToReturn.Id },
            characterToReturn);
        }

        /// <summary>
        /// PartiallyUpdate specified character with json patch.
        /// Supports upsertion.
        /// </summary>
        /// <remarks>
        /// Existing character id: 3810eba8-b61b-4b6b-8359-8b5480421042
        /// Requests must be placed in [ ] array.
        /// Sample request:
        ///
        ///     {
        ///         "op": "replace",
        ///         "path": "/name",
        ///         "value": "Replaced name"
        ///     },
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateCharacter(Guid id,
        [FromBody] JsonPatchDocument<CharacterForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var characterFromRepo = repository.GetCharacter(id);

            if (characterFromRepo == null)
                return UpsertCharacterFromPatch(id, patchDoc);

            var characterToPatch = Mapper.Map<CharacterForUpdateDto>(characterFromRepo);

            patchDoc.ApplyTo(characterToPatch, ModelState);
            TryValidateModel(characterToPatch);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            Mapper.Map(characterToPatch, characterFromRepo);

            Save(exceptionMessage: $"PATCH character {id} falied on save.");

            return NoContent();
        }

        private IActionResult UpsertCharacterFromPatch(Guid id, JsonPatchDocument<CharacterForUpdateDto> patchDoc)
        {
            var characterDto = new CharacterForUpdateDto();

            patchDoc.ApplyTo(characterDto, ModelState);

            ModelState.CheckModelForSameNameAndPlanet(characterDto);
            TryValidateModel(characterDto);
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            var characterToAdd = Mapper.Map<Character>(characterDto);

            characterToAdd.Id = id;

            repository.AddCharacter(characterToAdd);

            Save(exceptionMessage: $"Upserting character {id} failed on save.");

            var characterToReturn = Mapper.Map<CharacterDto>(characterToAdd);

            return CreatedAtRoute("GetCharacter",
                new { id = characterToReturn.Id },
                characterToReturn);
        }
    }
}