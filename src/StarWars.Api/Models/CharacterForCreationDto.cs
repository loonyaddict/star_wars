using StarWars.Api.Entities;
using System.Collections.Generic;

namespace StarWars.Api.Models
{
    public class CharacterForCreationDto
    {
        public string Name { get; set; }
        public string Planet { get; set; }
    }
}