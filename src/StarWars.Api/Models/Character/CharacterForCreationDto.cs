using StarWars.Api.Entities;
using System.Collections.Generic;

namespace StarWars.Api.Models
{
    public class CharacterForCreationDto : CharacterModelDto, ICharacter
    {
        public ICollection<Episode> Episodes { get; set; }
            = new List<Episode>();
    }
}