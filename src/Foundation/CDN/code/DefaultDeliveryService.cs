namespace Sitecore.Foundation.CDN
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Data.Items;

    public class DefaultDeliveryService : IDeliveryService
    {
        /// <summary>
        /// Sitecore Logger Implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Path Service for Generating Valid Paths to Items
        /// </summary>
        private readonly IPathService pathService;

        public DefaultDeliveryService(BaseLog logger, IPathService pathService)
        {
            this.logger = logger;
            this.pathService = pathService;
        }

        public virtual void Purge(IEnumerable<Item> items)
        {
            var list = items as Item[] ?? items.ToArray();

            if (!list.Any())
            {
                return;
            }

            var urls = list.SelectMany(item => this.pathService.GeneratePaths(item)).Distinct();

            foreach (var path in urls)
            {
                this.logger.Info($"Purge Request processing {path}", this);
            }
        }
    }
}