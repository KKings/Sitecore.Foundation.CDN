namespace Sitecore.Foundation.CDN.Jobs
{
    using System;
    using System.Linq;
    using Abstractions;
    using Data;
    using Pipelines.PurgeFilter;
    using Providers;
    using Sitecore.Jobs;

    public class PurgeJobRunner : IJobRunner
    {
        /// <summary>
        /// Sitecore Job Manager Implementation
        /// </summary>
        private readonly BaseJobManager jobManager;

        /// <summary>
        /// Sitecore Pipeline Manager Implementation
        /// </summary>
        private readonly BaseCorePipelineManager pipelineManager;

        /// <summary>
        /// Sitecore Logger Implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Service for accessing Sitecore History
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        /// Database Provider
        /// </summary>
        private readonly IDatabaseProvider databaseProvider;

        /// <summary>
        /// Delivery Service
        /// </summary>
        private readonly IDeliveryService deliveryService;

        public PurgeJobRunner(BaseJobManager jobManager,
            BaseCorePipelineManager pipelineManager,
            BaseLog logger,
            IHistoryService historyService,
            IDatabaseProvider databaseProvider,
            IDeliveryService deliveryService)
        {
            this.jobManager = jobManager;
            this.pipelineManager = pipelineManager;
            this.logger = logger;
            this.historyService = historyService;
            this.databaseProvider = databaseProvider;
            this.deliveryService = deliveryService;
        }

        /// <summary>
        /// Starts the Job
        /// </summary>
        /// <param name="name">The Job Name</param>
        /// <param name="category">The Job Category</param>
        /// <param name="siteName">SiteName for the Job</param>
        /// <param name="parameters">The Parameters to pass to the <see cref="ProcessJob"/> method</param>
        public virtual void Start(string name, string category, string siteName, params object[] parameters)
        {
            var options = new JobOptions(name, category, siteName, this, "ProcessJob", parameters);

            this.jobManager.Start(options);
        }

        /// <summary>
        /// Finds items that have chagned within the database that require a purge
        /// </summary>
        /// <param name="database">The database</param>
        public virtual void ProcessJob(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException($"{nameof(database)}");
            }

            var publishedItemIds = this.historyService.PublishHistory(database).ToArray();

            if (!publishedItemIds.Any())
            {
                this.logger.Info("PurgeJobScheduler was unable to find any items that need to be purged", this);
                return;
            }

            var publishedItems = publishedItemIds
                                    .Select(id => this.databaseProvider.ContentContext.GetItem(id))
                                    .Where(item => item != null);

            var args = new PurgeFilterAssetsArgs(publishedItems);

            this.pipelineManager.Run("cdn.purgeFilter", args);

            this.deliveryService.Purge(args.Output);
        }
    }
}