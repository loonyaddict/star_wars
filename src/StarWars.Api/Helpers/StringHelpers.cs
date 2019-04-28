using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarWars.Api.Helpers
{
    public static class StringHelpers
    {
        public static string TrimToLowerInvariant(string source) =>
            source.Trim().ToLowerInvariant();
    }
}
