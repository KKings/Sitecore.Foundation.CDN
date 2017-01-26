namespace Sitecore.Foundation.CDN
{
    public interface IDeliveryProvider
    {
        void Purge(string path);
    }
}
