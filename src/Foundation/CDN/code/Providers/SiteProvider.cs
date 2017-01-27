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
        /// Sitecore Configuration Factory Implementation
        /// </summary>
        private readonly BaseFactory factory;

        /// <summary>
        /// Database Provider Factory
        /// </summary>
        private readonly IDatabaseProvider databaseProvider;

        public SiteProvider(BaseFactory factory, IDatabaseProvider databaseProvider)
        {
            this.factory = factory;
            this.databaseProvider = databaseProvider;
        }

        /// <summary>
        /// Gets the Site in relation to an Item
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