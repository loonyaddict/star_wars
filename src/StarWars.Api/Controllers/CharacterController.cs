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
        private readonly IPropertyMappingService propertyMappingService;
        private readonly ITypeHelperService typeHelperService;

        public CharacterController(IStarWarsRepository repository,
            ILogger<CharacterController> logger,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)

            : base(repository)
        {
            this.logger = logger;
            this.urlHelper = urlHelper;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
        }

        [HttpGet("{id}", Name = "GetCharacter")]
        public IActionResult GetAuthor(Guid id)
        {
            var characterFromRepo = repository.GetCharacter(id);
            var authorToReturn = Mapper.Map<CharacterDto>(characterFromRepo);

            return Ok(characterFromRepo);
        }

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