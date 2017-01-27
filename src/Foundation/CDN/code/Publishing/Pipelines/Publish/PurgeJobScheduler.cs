namespace Sitecore.Foundation.CDN.Publishing.Pipelines.Publish
{
    using Abstractions;
    using Jobs;
    using Domain;
    using Sitecore.Publishing.Pipelines.Publish;

    public class PurgeJobScheduler : PublishProcessor
    {
        /// <summary>
        /// Sitecore logging implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Module Settings Repository
        /// </summary>
        private readonly ICdnSettings cdnSettings;

        /// <summary>
        /// Job runner for executing Jobs
        /// </summary>
        private readonly IJobRunner jobRunner;

        /// <summary>
        /// The History Service
        /// </summary>
        private readonly IHistoryService historyService;

        public PurgeJobScheduler(BaseLog logger, ICdnSettings cdnSettings, IJobRunner jobRunner, IHistoryService historyService)
        {
            this.logger = logger;
            this.cdnSettings = cdnSettings;
            this.jobRunner = jobRunner;
            this.historyService = historyService;
        }

        public override void Process(PublishContext context)
        {
            if (!this.cdnSettings.Enabled 
                || context?.PublishOptions?.TargetDatabase == null
                || context.PublishOptions.TargetDatabase.Name != this.cdnSettings.TargetDatabase)
            {
                this.logger.Error("Unable to schedule purge request.", this);
                return;
            }

            this.jobRunner.Start($"purge-{this.historyService.LastUpdateTime(context.PublishOptions.TargetDatabase)}",
                "purge",
                "shell",
                // Passed as a parameter to the job
                context.PublishOptions.TargetDatabase);
        }
    }
}