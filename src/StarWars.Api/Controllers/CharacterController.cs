using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using StarWars.Api.Entities;
using StarWars.Api.Models;
using StarWars.Api.Services;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Controllers
{
    [Route("api/characters")]
    public class CharacterController : Controller
    {
        private readonly IStarWarsRepository repository;
        private readonly ILogger<CharacterController> logger;

        private void Save(string exceptionMessage = "")
        {
            if (!repository.Save())
                throw new Exception(exceptionMessage);
        }

        public CharacterController(IStarWarsRepository repository, ILogger<CharacterController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetCharacters()
        {
            var charactersFromRepo = repository.Characters;
            var charactersToReturn = Mapper.Map<IEnumerable<CharacterDto>>(charactersFromRepo);

            return Ok(charactersToReturn);
        }

        [HttpGet("{id}", Name = "GetCharacter")]
        public IActionResult GetAuthor(Guid id)
        {
            var characterFromRepo = repository.GetCharacter(id);
            var authorToReturn = Mapper.Map<CharacterDto>(characterFromRepo);

            return Ok(characterFromRepo);
        }

        public IActionResult CreateCharacter([FromBody] CharacterForCreationDto characterToCreate)
        {
            if (characterToCreate == null)
                return BadRequest();

            CheckModelForSameNameAndPlanet(ModelState,
                characterToCreate.Name, characterToCreate.Planet);
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

            CheckModelForSameNameAndPlanet(ModelState,
                characterFromRepo.Name, characterFromRepo.Planet);
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

            CheckModelForSameNameAndPlanet(ModelState,
                characterToAdd.Name, characterToAdd.Planet);
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

            CheckModelForSameNameAndPlanet(ModelState,
                characterDto.Name, characterDto.Planet);

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

        #region Validation

        private void CheckModelForSameNameAndPlanet(ModelStateDictionary modelState,
            string name, string planet)
        {
            if (name == planet)
                modelState.AddModelError(name, "Name is same as planet.");
        }

        #endregion Validation
    }
}