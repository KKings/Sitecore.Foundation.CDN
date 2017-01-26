namespace Sitecore.Foundation.CDN.Providers
{
    using Data.Items;
    using Sites;

    public interface ISiteProvider
    {
        SiteContext GetSiteContext(Item item);
    }
}