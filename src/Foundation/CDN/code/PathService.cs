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
namespace Sitecore.Foundation.CDN
{
    using System;
    using System.Collections.Generic;
    using Abstractions;
    using Data.Items;
    using Links;
    using Providers;
    using Resources.Media;
    using Sites;

    public class PathService : IPathService
    {
        /// <summary>
        ///     Sitecore Link Manager
        /// </summary>
        private readonly BaseLinkManager linkManager;

        /// <summary>
        ///     Sitecore Media Manager
        /// </summary>
        private readonly BaseMediaManager mediaManager;

        /// <summary>
        ///     SiteContext Provider
        /// </summary>
        private readonly ISiteProvider siteProvider;

        public PathService(BaseLinkManager linkManager, BaseMediaManager mediaManager, ISiteProvider siteProvider)
        {
            this.linkManager = linkManager;
            this.mediaManager = mediaManager;
            this.siteProvider = siteProvider;
        }

        /// <summary>
        ///     Generate Path from an Item
        /// </summary>
        /// <param name="item">Sitecore </param>
        /// <returns>List of paths given an item</returns>
        public virtual IList<string> GeneratePaths(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException($"{nameof(item)}");
            }

            if (item.Paths.IsMediaItem)
            {
                var options = new MediaUrlOptions { AbsolutePath = false };

                return new[] { this.mediaManager.GetMediaUrl(item, options) };
            }

            using (new SiteContextSwitcher(this.siteProvider.GetSiteContext(item)))
            {
                var options = UrlOptions.DefaultOptions;

                options.AlwaysIncludeServerUrl = false;

                var pathToAsset = this.linkManager.GetItemUrl(item);

                if (pathToAsset.StartsWith($"{options.Site.Properties["scheme"]}://{options.Site.TargetHostName}"))
                {
                    pathToAsset = pathToAsset.Replace($"{options.Site.Properties["scheme"]}://{options.Site.TargetHostName}",
                        String.Empty);
                }

                var paths = new List<string> { pathToAsset };

                if (pathToAsset.EndsWith($"/{item.Language.Name}"))
                {
                    paths.Add("/");
                }

                return paths;
            }
        }
    }
}