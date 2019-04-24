using System;
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
        [MaxLength(5000)]
        public string Episodes { get; set; }
        [MaxLength(5000)]
        public string Friends { get; set; }

        //public ICollection<Episode> Episodes { get; set; }
        //    = new List<Episode>();

        ////todo friend character
        //public ICollection<Character> Friends { get; set; }
        //    = new List<Character>();
    }
}