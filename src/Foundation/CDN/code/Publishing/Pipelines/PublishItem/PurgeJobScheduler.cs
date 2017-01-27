namespace Sitecore.Foundation.CDN.Publishing.Pipelines.PublishItem
{
    using Abstractions;
    using CDN.Pipelines.PurgeFilter;
    using Data;
    using DependencyInjection;
    using Jobs;
    using Microsoft.Extensions.DependencyInjection;
    using Providers;
    using Sitecore.Publishing.Pipelines.PublishItem;

    public class PurgeJobScheduler : PublishItemProcessor
    {
        /// <summary>
        /// Sitecore logging implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Sitecore Pipeline Manager
        /// </summary>
        private readonly BaseCorePipelineManager pipelineManager;

        /// <summary>
        /// Site Provider
        /// </summary>
        private readonly ISiteProvider siteProvider;

        /// <summary>
        /// Job Runner
        /// </summary>
        protected IJobRunner JobRunner { get { return ServiceLocator.ServiceProvider.GetService<IJobRunner>(); } }

        public PurgeJobScheduler(BaseLog logger, BaseCorePipelineManager pipelineManager, ISiteProvider siteProvider)
        {
            this.logger = logger;
            this.pipelineManager = pipelineManager;
            this.siteProvider = siteProvider;
        }

        public override void Process(PublishItemContext context)
        {
/*            if (context == null || context.ItemId == ID.Null)
            {
                this.logger.Error("Unable to schedule purge request.", this);
                return;
            }

            var item = context.PublishOptions.SourceDatabase.GetItem(context.ItemId, context.PublishOptions.Language);

            if (item == null)
            {
                this.logger.Warn($"Unable to schedule purge request for {context.ItemId}.", this);
                return;
            }

            var siteContext = this.siteProvider.GetSiteContext(item);

            if (siteContext == null)
            {
                this.logger.Warn($"Unable to schedule purge request without a SiteContext for {item.ID}.", this);
                return;
            }

            var args = new PurgeFilterAssetsArgs(siteContext, item);

            this.pipelineManager.Run("cdn.purgeFilter", args);

            if (!args.IsAllowed)
            {
                return;
            }

            this.JobRunner.Start($"purge-{context.ItemId}",
                "purge",
                "shell",
                // Passed as a parameter to the job
                item,
                siteContext);*/
        }
    }
}