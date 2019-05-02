using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarWars.Api.Entities
{
    /// <summary>
    /// Character entity.
    /// </summary>
    public class Character : ICharacter
    {
        /// <summary>
        /// Id in database
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Character name.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Character planet.
        /// </summary>
        [MaxLength(50)]
        public string Planet { get; set; }

        /// <summary>
        /// Charaters episodes.
        /// </summary>
        public ICollection<Episode> Episodes { get; set; }
            = new List<Episode>();
    }
}