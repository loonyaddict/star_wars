using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StarWars.Api.Helpers
{
    public static class IEnumerableHelpers
    {
        public static void AddMany<T>(this ICollection<T> source,
            params T[] items)
        {
            Debug.Assert(
            source.All(item =>
            item.GetType() == typeof(T)));

            foreach (var item in items)
                source.Add(item);
        }
    }
}