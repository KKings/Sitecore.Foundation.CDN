namespace Sitecore.Foundation.CDN.Domain
{
    public class CdnSettings : ICdnSettings
    {
        /// <summary>
        /// Gets if the Module is enabled
        /// </summary>
        public virtual bool Enabled { get { return Configuration.Settings.GetBoolSetting("CDN.Enabled", false); } }

        /// <summary>
        /// Gets the Target Database that will filter publish events on
        /// </summary>
        public string TargetDatabase { get { return Configuration.Settings.GetSetting("CDN.TargetDatabase", "web");  } }
    }
}