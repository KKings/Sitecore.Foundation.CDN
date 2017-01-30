namespace Sitecore.Foundation.CDN.Http
{
    using System;
    using Newtonsoft.Json;

    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// Serializes a string into an object of type <see cref="T"/>
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>String representation of type <see cref="T"/></returns>
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Deserializes a string into an object of type <see cref="T"/>
        /// </summary>
        /// <param name="value">The value is serialize</param>
        /// <returns>Object of type <see cref="T"/></returns>
        public T Deserialize<T>(string value)
        {
            return String.IsNullOrEmpty(value) ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}