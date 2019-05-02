using StarWars.Api.Entities;
using System;

namespace StarWars.Api.Models
{
    /// <summary>
    /// Model to be returned to user by default.
    /// </summary>
    public class CharacterDto : CharacterModelDto, ICharacter
    {
        /// <summary>
        /// Character Id in database.
        /// </summary>
        public Guid Id { get; set; }
    }
}