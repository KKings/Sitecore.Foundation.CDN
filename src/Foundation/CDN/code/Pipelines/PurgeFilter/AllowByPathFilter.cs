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
            var allowed =
                args.Input.Where(
                    item => this.AllowedPaths.Any(path => item.Paths.FullPath.StartsWith(path, true, CultureInfo.InvariantCulture)));

            args.Output = allowed;
        }
    }
}