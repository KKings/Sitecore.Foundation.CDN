namespace Sitecore.Foundation.CDN.Pipelines.PurgeFilter
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class AllowByPathFilter
    {
        /// <summary>
        /// Gets the Allowed Paths
        /// </summary>
        public List<string> AllowedPaths { get; set; } = new List<string>();

        public void Process(PurgeFilterAssetsArgs args)
        {
            var isAllowed = this.AllowedPaths.Any(path => args.Item.Paths.FullPath.StartsWith(path, true, CultureInfo.InvariantCulture));

            if (!isAllowed)
            {
                args.IsAllowed = false;
                args.AbortPipeline();
            }
        }
    }
}