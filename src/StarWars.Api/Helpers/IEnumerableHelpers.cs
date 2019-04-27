using System.Collections.Generic;

namespace StarWars.Api.Helpers
{
    public static class IEnumerableHelpers
    {
        public static void AddMany<T>(this ICollection<T> source, params T[] items)
        {
            foreach (var item in items)
                source.Add(item);
        }
    }
}