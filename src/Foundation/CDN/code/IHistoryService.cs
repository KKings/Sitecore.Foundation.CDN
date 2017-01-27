namespace Sitecore.Foundation.CDN
{
    using System;
    using System.Collections.Generic;
    using Data;

    public interface IHistoryService
    {
        IEnumerable<ID> PublishHistory(Database database);

        DateTime LastUpdateTime(Database database);
    }
}