namespace Sitecore.Foundation.CDN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Data;

    public class HistoryService : IHistoryService
    {
        /// <summary>
        /// Database Property for the last update
        /// </summary>
        const string LastUpdateProperty = "CdnLastUpdate";

        /// <summary>
        /// Sitecore History Manager Implementation
        /// </summary>
        private readonly BaseHistoryManager historyManager;

        /// <summary>
        /// Gets the Current Time
        /// </summary>
        public virtual DateTime Now => DateTime.UtcNow;

        public HistoryService(BaseHistoryManager historyManager)
        {
            this.historyManager = historyManager;
        }

        /// <summary>
        /// Gets the Publish History for a Database
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public virtual IEnumerable<ID> PublishHistory(Database database)
        {
            var now = this.Now;
            var from = this.LastUpdateTime(database);

            // Gets the entrys from the history manager using the database
            var entrys = this.historyManager.GetHistory(database, from, now);

            if (!entrys.Any())
            {
                return new ID[0];
            }

            var queue = new List<ID>();

            foreach (
                var entry in
                    entrys.Where(
                        entry => !queue.Contains(entry.ItemId) && entry.Category == Data.Engines.HistoryCategory.Item))
            {
                queue.Add(entry.ItemId);
                database.Properties[HistoryService.LastUpdateProperty] = DateUtil.ToIsoDate(entry.Created, true);
            }

            database.Properties[HistoryService.LastUpdateProperty] = DateUtil.ToIsoDate(now, true);

            return queue;
        }

        /// <summary>
        /// Returns the Last Update Time from a database
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public virtual DateTime LastUpdateTime(Database database)
        {
            var lastUpdate = database.Properties[HistoryService.LastUpdateProperty];

            return lastUpdate.Length > 0 ? DateUtil.ParseDateTime(lastUpdate, DateTime.MinValue) : DateTime.MinValue;
        }
    }
}