// MIT License
// 
// Copyright (c) 2017 Kyle Kingsbury
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace Sitecore.Foundation.CDN.Http
{
    using System;
    using Newtonsoft.Json;

    public class JsonSerializer : ISerializer
    {
        /// <summary>
        ///     Serializes a string into an object of type <see cref="T" />
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>String representation of type <see cref="T" /></returns>
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        ///     Deserializes a string into an object of type <see cref="T" />
        /// </summary>
        /// <param name="value">The value is serialize</param>
        /// <returns>Object of type <see cref="T" /></returns>
        public T Deserialize<T>(string value)
        {
            return String.IsNullOrEmpty(value) ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}