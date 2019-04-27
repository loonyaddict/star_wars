using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Models
{
    public abstract class CharacterModelDto
    {

        [Required(ErrorMessage = "Name cannot be empty")]
        [MaxLength(50, ErrorMessage = "Max name lenght: 50")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "Max planet lenght: 50")]
        public string Planet { get; set; }
    }
}
