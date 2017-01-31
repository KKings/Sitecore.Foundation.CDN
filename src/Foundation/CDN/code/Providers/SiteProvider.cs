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
namespace Sitecore.Foundation.CDN.Providers
{
    using System;
    using System.Linq;
    using Abstractions;
    using Data.Items;
    using SecurityModel;
    using Sites;

    public class SiteProvider : ISiteProvider
    {
        /// <summary>
        ///     Database Provider Factory
        /// </summary>
        private readonly IDatabaseProvider databaseProvider;

        /// <summary>
        ///     Sitecore Configuration Factory Implementation
        /// </summary>
        private readonly BaseFactory factory;

        public SiteProvider(BaseFactory factory, IDatabaseProvider databaseProvider)
        {
            this.factory = factory;
            this.databaseProvider = databaseProvider;
        }

        /// <summary>
        ///     Gets the Site in relation to an Item
        /// </summary>
        /// <param name="item">The Item</param>
        /// <returns><c>SiteContext</c> for the item or the Shell site</returns>
        public virtual SiteContext GetSiteContext(Item item)
        {
            if (item == null)
            {
                return SiteContext.GetSite("shell");
            }

            // Loop through all configured sites
            foreach (
                var site in
                    this.factory.GetSiteInfoList()
                        .Where(s => !s.Database.Equals("core", StringComparison.InvariantCultureIgnoreCase))
                        .Reverse())
            {
                using (new SecurityDisabler())
                {
                    // Get this site's home page item
                    var homePage = this.databaseProvider.ContentContext.GetItem(site.RootPath + site.StartItem);

                    // if the item lives within this site, this is our context site
                    if (homePage != null
                        // Since the multisite approach is to have a container node, check to see if this item is within either
                        && (homePage.Axes.IsAncestorOf(item) || homePage.Parent.Axes.IsAncestorOf(item) || homePage.ID == item.ID)
                        && homePage.Paths.FullPath != "/sitecore"
                        && homePage.Paths.FullPath != "/sitecore/content")
                    {
                        return this.factory.GetSite(site.Name);
                    }
                }
            }

            return SiteContext.GetSite("shell");
        }
    }
}