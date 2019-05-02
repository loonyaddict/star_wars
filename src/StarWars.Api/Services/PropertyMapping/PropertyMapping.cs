using System.Collections.Generic;

namespace StarWars.API.Services
{
    /// <summary>
    /// Class for manipulation of PropertyMappings.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        /// <summary>
        /// Mapping dictionary.
        /// </summary>
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="mappingDictionary"></param>
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
        }
    }
}