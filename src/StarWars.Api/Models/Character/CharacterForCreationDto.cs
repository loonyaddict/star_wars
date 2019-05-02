using StarWars.Api.Entities;
using System.Collections.Generic;

namespace StarWars.Api.Models
{
    /// <summary>
    /// Model for character creation.
    /// </summary>
    public class CharacterForCreationDto : CharacterModelDto, ICharacter
    {
        /// <summary>
        /// Enables adding episodes in one request.
        /// </summary>
        public ICollection<Episode> Episodes { get; set; }
            = new List<Episode>();
    }
}