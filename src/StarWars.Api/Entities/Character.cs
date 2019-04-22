using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        public ICollection<Character> Friends { get; set; } =
            new List<Character>();
    }
}
