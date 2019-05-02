using System;

namespace StarWars.Api.Models
{
    /// <summary>
    /// Model to be returned to user by default.
    /// </summary>
    public class EpisodeDto
    {
        /// <summary>
        /// Id of episode in database
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Episode name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Reference to parent Character.
        /// </summary>
        public Guid CharacterId { get; set; }
    }
}