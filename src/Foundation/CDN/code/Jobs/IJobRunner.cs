namespace Sitecore.Foundation.CDN.Jobs
{
    using Data;
    using Data.Items;
    using Globalization;
    using Sites;

    public interface IJobRunner
    {
        void Start(string name, string category, string siteName, params object[] parameters);

        void ProcessJob(Item item, SiteContext siteContext);
    }
}