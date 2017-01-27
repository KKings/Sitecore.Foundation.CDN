namespace Sitecore.Foundation.CDN
{
    using System.Collections.Generic;
    using Data.Items;

    public interface IDeliveryService
    {
        void Purge(IEnumerable<Item> Items);
    }
}
