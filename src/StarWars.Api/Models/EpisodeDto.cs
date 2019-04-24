using System;

namespace StarWars.Api.Models
{
    public class EpisodeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Guid CharacterId { get; set; }
    }
}