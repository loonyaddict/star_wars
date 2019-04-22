using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StarWars.Api.Entities
{
    public class Character
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Planet { get; set; }

        public ICollection<Episode> Episodes { get; set; }
            = new List<Episode>();

        public ICollection<Character> Friends { get; set; }
            = new List<Character>();
    }
}