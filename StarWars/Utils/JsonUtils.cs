using Newtonsoft.Json;
using System.Diagnostics;

namespace StarWars.Utils.JsonUtils
{
    internal static class JsonUtils
    {
        internal static string ToJson(this object @object)
        {
            Debug.Assert(@object != null, "ToJson() called on null object!");
            return JsonConvert.SerializeObject(
                @object, StarWarsSettings.Instance.NullIgnoreSettings);
        }
    }
}