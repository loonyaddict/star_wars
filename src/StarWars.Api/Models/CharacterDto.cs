using System;
using System.Collections;
using System.Collections.Generic;

namespace StarWars.Api.Models
{
    public class CharacterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Planet { get; set; }

        public ICollection<string> Friends { get; set; }
            = new List<string>();
    }
}