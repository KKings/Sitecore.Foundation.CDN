namespace Sitecore.Foundation.CDN.Media
{
    using System;
    using Data.Items;
    using Domain;
    using Resources.Media;

    public class CdnMediaProvider : MediaProvider
    {
        /// <summary>
        /// The Cdn Settings
        /// </summary>
        private readonly ICdnSettings cdnSettings;

        public CdnMediaProvider(ICdnSettings cdnSettings)
        {
            this.cdnSettings = cdnSettings;
        }

        /// <summary>
        /// Overrides the <see cref="MediaProvider"/> and investigates if the Cdn Target Database is the current 
        /// database, if not removes the cdn prefix
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="options">The options</param>
        /// <returns>The Media Url</returns>
        public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
        {
            if (this.cdnSettings.Enabled
                && item.Database.Name != this.cdnSettings.TargetDatabase 
                && options.AlwaysIncludeServerUrl 
                && !String.IsNullOrEmpty(options.MediaLinkServerUrl))
            {
                options.AlwaysIncludeServerUrl = false;
            }

            return base.GetMediaUrl(item, options);
        }
    }
}