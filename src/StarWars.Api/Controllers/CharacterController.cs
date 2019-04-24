using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Entities;
using StarWars.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StarWars.Api.Models;

namespace StarWars.Api.Controllers
{
    [Route("api/characters")]
    public class CharacterController : Controller
    {
        private readonly IStarWarsRepository repository;

        public CharacterController(IStarWarsRepository repository) =>
            this.repository = repository;

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

        [HttpPost]
        public IActionResult CreateCharacter([FromBody] CharacterForCreationDto characterToCreate)
        {
            if (characterToCreate == null)
                return BadRequest();
            var character = Mapper.Map<Character>(characterToCreate);
            repository.AddCharacter(character);
            
            if (!repository.Save())
                return StatusCode(500,
                    "Request could not be handled at the moment. Try again later.");

            var characterToReturn = Mapper.Map<CharacterDto>(character);

            return CreatedAtRoute("GetCharacter",
                new
                {
                    id = character.Id
                },
                characterToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCharacter(Guid id)
        {
            var characterFromRepo = repository.GetCharacter(id);

            if (characterFromRepo == null)
                return NotFound();

            repository.DeleteCharacter(characterFromRepo);

            if (!repository.Save())
                throw new Exception($"Deleting character {id} falied on save.");

            return NoContent();
        }
    }
}
