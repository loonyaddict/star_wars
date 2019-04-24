using StarWars.Api.Entities;
using System;
using System.Collections.Generic;

namespace StarWars.Api.Models
{
    public class CharacterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Planet { get; set; }
        public string Episodes { get; set; }
        public string Friends { get; set; }

    }
}