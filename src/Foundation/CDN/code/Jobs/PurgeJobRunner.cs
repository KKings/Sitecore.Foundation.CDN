// MIT License
// 
// Copyright (c) 2017 Kyle Kingsbury
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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
        ///     Database Provider
        /// </summary>
        private readonly IDatabaseProvider databaseProvider;

        /// <summary>
        ///     Delivery Service
        /// </summary>
        private readonly IDeliveryService deliveryService;

        /// <summary>
        ///     Service for accessing Sitecore History
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        ///     Sitecore Job Manager Implementation
        /// </summary>
        private readonly BaseJobManager jobManager;

        /// <summary>
        ///     Sitecore Logger Implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        ///     Sitecore Pipeline Manager Implementation
        /// </summary>
        private readonly BaseCorePipelineManager pipelineManager;

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
        ///     Starts the Job
        /// </summary>
        /// <param name="name">The Job Name</param>
        /// <param name="category">The Job Category</param>
        /// <param name="siteName">SiteName for the Job</param>
        /// <param name="parameters">The Parameters to pass to the <see cref="ProcessJob" /> method</param>
        public virtual void Start(string name, string category, string siteName, params object[] parameters)
        {
            var options = new JobOptions(name, category, siteName, this, "ProcessJob", parameters);

            this.jobManager.Start(options);
        }

        /// <summary>
        ///     Finds items that have chagned within the database that require a purge
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