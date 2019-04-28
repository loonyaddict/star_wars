using System;

namespace StarWars.Api.Models
{
    public class CharacterDto : CharacterModelDto
    {
        public Guid Id { get; set; }
    }
}