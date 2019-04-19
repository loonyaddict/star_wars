using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace StarWars.Utils.JsonUtils
{
    internal class StarWarsSettings
    {
        private readonly JsonSerializerSettings nullIgnoreSettings =
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

        internal JsonSerializerSettings NullIgnoreSettings => nullIgnoreSettings;

        #region Singleton

        private StarWarsSettings() => Debug.WriteLine($"Singleton Settings Created");

        private static readonly Lazy<StarWarsSettings> lazy = new Lazy<StarWarsSettings>(() => new StarWarsSettings());

        internal static StarWarsSettings Instance => lazy.Value;

        #endregion Singleton
    }
}