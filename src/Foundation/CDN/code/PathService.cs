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
        private readonly BaseLinkManager linkManager;
        private readonly BaseMediaManager mediaManager;
        private readonly ISiteProvider siteProvider;

        public PathService(BaseLinkManager linkManager, BaseMediaManager mediaManager, ISiteProvider siteProvider)
        {
            this.linkManager = linkManager;
            this.mediaManager = mediaManager;
            this.siteProvider = siteProvider;
        }

        public virtual IList<string> GeneratePaths(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException($"{nameof(item)}");
            }

            if (item.Paths.IsMediaItem)
            {
                var options = new MediaUrlOptions { AbsolutePath = false };

                return new [] { this.mediaManager.GetMediaUrl(item, options) };
            }

            using (new SiteContextSwitcher(this.siteProvider.GetSiteContext(item)))
            {
                var options = UrlOptions.DefaultOptions;

                options.AlwaysIncludeServerUrl = false;

                var pathToAsset = this.linkManager.GetItemUrl(item);

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