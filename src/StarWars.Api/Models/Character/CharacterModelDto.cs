using Microsoft.AspNetCore.Identity;
using StarWars.Api.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Models
{
    /// <summary>
    /// Base class for CharacterModel
    /// </summary>
    public abstract class CharacterModelDto : ICharacter
    {
        /// <summary>
        /// Character name
        /// </summary>
        [Required(ErrorMessage = "Name cannot be empty")]
        [MaxLength(50, ErrorMessage = "Max name lenght: 50")]
        public string Name { get; set; }

        /// <summary>
        /// Character planet
        /// </summary>
        [MaxLength(50, ErrorMessage = "Max planet lenght: 50")]
        public string Planet { get; set; }
    }
}
