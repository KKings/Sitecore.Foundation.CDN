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
namespace Sitecore.Foundation.CDN.Media
{
    using System;
    using Data.Items;
    using Domain;
    using Resources.Media;

    public class CdnMediaProvider : MediaProvider
    {
        /// <summary>
        ///     The Cdn Settings
        /// </summary>
        private readonly ICdnSettings cdnSettings;

        public CdnMediaProvider(ICdnSettings cdnSettings)
        {
            this.cdnSettings = cdnSettings;
        }

        /// <summary>
        ///     Overrides the <see cref="MediaProvider" /> and investigates if the Cdn Target Database is the current
        ///     database, if not removes the cdn prefix
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="options">The options</param>
        /// <returns>The Media Url</returns>
        public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
        {
            if (this.cdnSettings.Enabled
                && item.Database.Name != this.cdnSettings.TargetDatabase
                && options.AlwaysIncludeServerUrl
                && !String.IsNullOrEmpty(options.MediaLinkServerUrl))
            {
                options.AlwaysIncludeServerUrl = false;
            }

            return base.GetMediaUrl(item, options);
        }
    }
}