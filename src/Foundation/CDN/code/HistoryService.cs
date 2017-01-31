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
namespace Sitecore.Foundation.CDN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Data;
    using Data.Engines;

    public class HistoryService : IHistoryService
    {
        /// <summary>
        ///     Database Property for the last update
        /// </summary>
        private const string LastUpdateProperty = "CdnLastUpdate";

        /// <summary>
        ///     Sitecore History Manager Implementation
        /// </summary>
        private readonly BaseHistoryManager historyManager;

        public HistoryService(BaseHistoryManager historyManager)
        {
            this.historyManager = historyManager;
        }

        /// <summary>
        ///     Gets the Current Time
        /// </summary>
        public virtual DateTime Now => DateTime.UtcNow;

        /// <summary>
        ///     Gets the Publish History for a Database
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
                        entry => !queue.Contains(entry.ItemId) && entry.Category == HistoryCategory.Item))
            {
                queue.Add(entry.ItemId);
                database.Properties[HistoryService.LastUpdateProperty] = DateUtil.ToIsoDate(entry.Created, true);
            }

            database.Properties[HistoryService.LastUpdateProperty] = DateUtil.ToIsoDate(now, true);

            return queue;
        }

        /// <summary>
        ///     Returns the Last Update Time from a database
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