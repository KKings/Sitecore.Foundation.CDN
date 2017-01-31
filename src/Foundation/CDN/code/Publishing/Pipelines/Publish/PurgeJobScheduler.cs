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
namespace Sitecore.Foundation.CDN.Publishing.Pipelines.Publish
{
    using Abstractions;
    using Domain;
    using Jobs;
    using Sitecore.Publishing.Pipelines.Publish;

    public class PurgeJobScheduler : PublishProcessor
    {
        /// <summary>
        ///     Module Settings Repository
        /// </summary>
        private readonly ICdnSettings cdnSettings;

        /// <summary>
        ///     The History Service
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        ///     Job runner for executing Jobs
        /// </summary>
        private readonly IJobRunner jobRunner;

        /// <summary>
        ///     Sitecore logging implementation
        /// </summary>
        private readonly BaseLog logger;

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