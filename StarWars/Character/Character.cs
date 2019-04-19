using Newtonsoft.Json;
using System.Collections.Generic;

namespace StarWars.Characters
{
    /// <summary>
    /// Star wars character.
    /// </summary>
    public class MovieCharacter
    {
        public MovieCharacter(string name)
        {
            Name = name;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("planet")]
        public string HomePlanet { get; set; }

        [JsonProperty("episodes")]
        public List<string> Episodes { get; set; }

        [JsonProperty("friends")]
        public List<string> Friends { get; set; }
    }
}