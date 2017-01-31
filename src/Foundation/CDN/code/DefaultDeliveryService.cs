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
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Data.Items;

    public class DefaultDeliveryService : IDeliveryService
    {
        /// <summary>
        ///     Sitecore Logger Implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        ///     Path Service for Generating Valid Paths to Items
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