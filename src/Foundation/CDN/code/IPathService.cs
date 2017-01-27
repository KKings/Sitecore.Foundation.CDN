namespace Sitecore.Foundation.CDN
{
    using System.Collections.Generic;
    using Data.Items;

    public interface IPathService
    {
        IList<string> GeneratePaths(Item item);
    }
}