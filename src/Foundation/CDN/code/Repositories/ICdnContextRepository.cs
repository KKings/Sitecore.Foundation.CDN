namespace Sitecore.Foundation.CDN.Repositories
{
    using Domain;
    using Sites;

    public interface ICdnContextRepository
    {
        CdnContext Get(SiteContext siteContext);
    }
}
