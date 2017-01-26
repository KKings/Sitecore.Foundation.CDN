namespace Sitecore.Foundation.CDN.Pipelines.PurgeFilter
{
    using Data.Items;
    using Sitecore.Pipelines;
    using Sites;

    public class PurgeFilterAssetsArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the SiteContext
        /// </summary>
        public SiteContext SiteContext { get; private set; }

        public Item Item { get; private set; }

        /// <summary>
        /// Gets or sets if the Item passed within the args should be filtered by the caller
        /// </summary>
        public bool IsAllowed { get; set; } = true;

        public PurgeFilterAssetsArgs(SiteContext siteContext, Item item)
        {
            this.SiteContext = siteContext;
            this.Item = item;
        }
    }
}