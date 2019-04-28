using StarWars.Api.Entities;
using System;

namespace StarWars.Api.Models
{
    public class CharacterDto : CharacterModelDto, ICharacter
    {
        public Guid Id { get; set; }
    }
}