namespace Sitecore.Foundation.CDN.Media
{
    using System;
    using Domain;
    using Providers;
    using Resources.Media;
    using Abstractions;
    using Data.Items;
    using DependencyInjection;
    using Events.Hooks;
    using Microsoft.Extensions.DependencyInjection;

    public class CdnMediaManager : DefaultMediaManager, IHook
    {
        /// <summary>
        /// The Cdn Settings
        /// </summary>
        private readonly ICdnSettings settings;

        /// <summary>
        /// Database Provider
        /// </summary>
        private IDatabaseProvider databaseProvider { get { return ServiceLocator.ServiceProvider.GetService<IDatabaseProvider>(); } }

        public CdnMediaManager(BaseFactory factory, MediaProvider provider, ICdnSettings settings) : base(factory, provider)
        {
            this.settings = settings;
        }

        public override String GetMediaUrl(MediaItem item, MediaUrlOptions options)
        {
            return base.GetMediaUrl(item, options);
        }

        public override Media GetMedia(MediaItem item)
        {
            return base.GetMedia(item);
        }

        public override string MediaLinkPrefix
        {
            get
            {
                if (this.databaseProvider.Context.Name == this.settings.TargetDatabase)
                {
                    return String.Empty; //Todo: Add Originprefix this.OriginPrefix;
                }

                return this.Config.MediaLinkPrefix;
            }
        }

        public void Initialize()
        {
            var config = MediaManager.Config;
        }
    }
}