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
namespace Sitecore.Foundation.CDN.Domain
{
    using System;
    using System.Collections.Generic;

    public class CdnContext
    {
        private const string EnabledProperty = "cdnEnabled";

        private const string UrlProperty = "cdnUrl";

        /// <summary>
        ///     Gets the Domain of the CDN
        /// </summary>
        public string Url
        {
            get
            {
                return this.Settings.ContainsKey(CdnContext.UrlProperty) ? this.Settings[CdnContext.UrlProperty] : String.Empty;
            }
        }

        /// <summary>
        ///     Gets or sets if the module for this context is enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.Settings.ContainsKey(CdnContext.EnabledProperty) &&
                       this.Settings[CdnContext.EnabledProperty].Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        ///     Gets or sets the Settings
        /// </summary>
        public IDictionary<string, string> Settings { get; } = new Dictionary<string, string>();
    }
}