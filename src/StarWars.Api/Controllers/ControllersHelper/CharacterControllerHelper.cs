using Microsoft.AspNetCore.Mvc.ModelBinding;
using StarWars.Api.Entities;

namespace StarWars.Api.Controllers.ControllersHelper.CharacterHelper
{
    public static class CharacterControllerHelper
    {
        /// <summary>
        /// Custom rule: Name should not be same as plent. Needs to be called on create and update operations.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="character"></param>
        public static void CheckModelForSameNameAndPlanet(this ModelStateDictionary modelState, ICharacter character)
        {
            if (character.Name == character.Planet)
            {
                modelState.AddModelError(
                    $"{character.Name}_{character.Planet}",
                    "Name should not be same as planet.");
            }
        }
    }
}