using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarWars.Api.Entities
{
    /// <summary>
    /// Episode entity
    /// </summary>
    public class Episode
    {
        /// <summary>
        /// Key in database.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Episode name.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Reference to parent.
        /// </summary>
        [ForeignKey("CharacterId")]
        public Character Character { get; set; }

        /// <summary>
        /// Reference to parentId used by database for faster search.
        /// </summary>
        public Guid CharacterId { get; set; }
    }
}