namespace Sitecore.Foundation.CDN.Pipelines.PurgeFilter
{
    using System.Collections.Generic;
    using Data.Items;
    using Sitecore.Pipelines;
    using Sites;

    public class PurgeFilterAssetsArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the Items
        /// </summary>
        public IEnumerable<Item> Input { get; private set; }
    
        /// <summary>
        /// Gets the Items that were not filtered
        /// </summary>
        public IEnumerable<Item> Output { get; set; } = new List<Item>();

        public PurgeFilterAssetsArgs(IEnumerable<Item> input)
        {
            this.Input = input;
        }
    }
}