using Microsoft.AspNetCore.Mvc;
using StarWars.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            //var characters = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);

            return Ok(charactersFromRepo);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            var characterFromRepo = repository.GetCharacter(id);

            return Ok(characterFromRepo);
        }
        
    }
}
