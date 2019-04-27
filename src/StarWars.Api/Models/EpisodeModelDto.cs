using System.ComponentModel.DataAnnotations;

namespace StarWars.Api.Models
{
    public abstract class EpisodeModelDto
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        [MaxLength(50, ErrorMessage = "Max name lenght: 50")]
        public string Name { get; set; }
    }
}