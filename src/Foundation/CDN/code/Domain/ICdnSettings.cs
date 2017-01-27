namespace Sitecore.Foundation.CDN.Domain
{
    public interface ICdnSettings
    {
        bool Enabled { get; }
        string TargetDatabase { get; }
    }
}