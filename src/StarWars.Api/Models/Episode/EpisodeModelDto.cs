using System.ComponentModel.DataAnnotations;

namespace StarWars.Api.Models
{
    /// <summary>
    /// Base class for Episode model
    /// </summary>
    public abstract class EpisodeModelDto
    {
        /// <summary>
        /// Episode name
        /// </summary>
        [Required(ErrorMessage = "Name cannot be empty")]
        [MaxLength(50, ErrorMessage = "Max name lenght: 50")]
        public string Name { get; set; }
    }
}