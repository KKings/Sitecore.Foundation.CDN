namespace Sitecore.Foundation.CDN.Repositories
{
    using System;
    using System.Linq;
    using Domain;
    using Sites;

    public class CdnContextRepository : ICdnContextRepository
    {
        const string PropertyPrefix = "cdn";

        public virtual CdnContext Get(SiteContext siteContext)
        {
            if (siteContext == null)
            {
                throw new ArgumentNullException($"{nameof(siteContext)}");
            }
            
            var context = new CdnContext();

            foreach (
                var key in siteContext.Properties.AllKeys.Where(key => key.StartsWith(CdnContextRepository.PropertyPrefix)))
            {
                if (!context.Settings.ContainsKey(key))
                {
                    context.Settings.Add(key, siteContext.Properties[key]);
                }
            }

            return context;
        }
    }
}