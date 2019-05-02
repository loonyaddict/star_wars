using System.Collections.Generic;

namespace StarWars.API.Services
{
    /// <summary>
    /// Base class for PropertyMapping manipulation.
    /// </summary>
    public class PropertyMappingValue
    {
        /// <summary>
        /// Properties for destination
        /// </summary>
        public IEnumerable<string> DestinationProperties { get; private set; }

        /// <summary>
        /// Checks in collection order needs to be reverted before returning.
        /// </summary>
        public bool Revert { get; private set; }

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="destinationProperties"></param>
        /// <param name="revert"></param>
        public PropertyMappingValue(IEnumerable<string> destinationProperties,
            bool revert = false)
        {
            DestinationProperties = destinationProperties;
            Revert = revert;
        }
    }
}