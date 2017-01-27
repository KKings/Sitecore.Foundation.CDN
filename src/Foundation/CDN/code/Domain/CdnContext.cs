namespace Sitecore.Foundation.CDN.Domain
{
    using System;
    using System.Collections.Generic;

    public class CdnContext
    {
        const string EnabledProperty = "cdnEnabled";

        const string UrlProperty = "cdnUrl";

        /// <summary>
        /// Gets the Domain of the CDN
        /// </summary>
        public string Url { get { return this.Settings.ContainsKey(CdnContext.UrlProperty) ? this.Settings[CdnContext.UrlProperty] : String.Empty; } }

        /// <summary>
        /// Gets or sets if the module for this context is enabled
        /// </summary>
        public bool Enabled { get { return this.Settings.ContainsKey(CdnContext.EnabledProperty) && this.Settings[CdnContext.EnabledProperty].Equals("true", StringComparison.InvariantCultureIgnoreCase); } }

        /// <summary>
        /// Gets or sets the Settings
        /// </summary>
        public IDictionary<string, string> Settings { get; } = new Dictionary<string, string>();
    }
}