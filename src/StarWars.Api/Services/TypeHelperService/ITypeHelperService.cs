using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.API.Services
{
    /// <summary>
    /// Interface for service injection.
    /// </summary>
    public interface ITypeHelperService
    {
        /// <summary>
        /// Checks if string fields cand be parsed to Model Properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <returns></returns>
        bool TypeHasProperties<T>(string fields);
    }
}
