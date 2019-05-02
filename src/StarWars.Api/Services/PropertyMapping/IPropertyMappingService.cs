using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.API.Services
{
    /// <summary>
    /// Interface for service injection
    /// </summary>
    public interface IPropertyMappingService
    {
        /// <summary>
        /// Checks if Mapping can be created for specified source and destination.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="fields"></param>
        /// <returns></returns>
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);

        /// <summary>
        /// Should only be called after checking if ValidMappingExistsFor
        /// Gets property mapping for specified source and destination.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
