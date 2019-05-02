using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Helpers
{
    internal static class StringHelpers
    {
        internal static string TrimToLowerInvariant(string source) =>
            source.Trim().ToLowerInvariant();
    }
}
