namespace StarWars.Api.Entities
{
    /// <summary>
    /// ICharacter interface for code reusability.
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// Character name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Character planet.
        /// </summary>
        string Planet { get; set; }
    }
}