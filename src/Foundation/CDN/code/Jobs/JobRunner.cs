namespace Sitecore.Foundation.CDN.Jobs
{
    using Abstractions;
    using Data.Items;
    using Links;
    using Sitecore.Jobs;
    using Sites;

    public class JobRunner : IJobRunner
    {
        /// <summary>
        /// Sitecore Job Manager Implementation
        /// </summary>
        private readonly BaseJobManager jobManager;

        /// <summary>
        /// Sitecore Logging Implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Sitecore Link Manager Implementation
        /// </summary>
        private readonly BaseLinkManager linkManager;

        public JobRunner(BaseJobManager jobManager, BaseLog logger, BaseLinkManager linkManager)
        { 
            this.jobManager = jobManager;
            this.logger = logger;
            this.linkManager = linkManager;
        }

        public virtual void Start(string name, string category, string siteName, params object[] parameters)
        {
            var options = new JobOptions(name, category, siteName, this, "ProcessJob", parameters);

            this.jobManager.Start(options);
        }

        public virtual void ProcessJob(Item item, SiteContext siteContext)
        {
            if (item == null)
            {
                return;
            }

            using (new SiteContextSwitcher(siteContext))
            {
                var options = UrlOptions.DefaultOptions;

                options.AlwaysIncludeServerUrl = false;

                var pathToAsset = this.linkManager.GetItemUrl(item);
            }
        }
    }
}