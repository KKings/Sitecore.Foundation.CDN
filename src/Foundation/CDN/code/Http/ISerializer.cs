namespace Sitecore.Foundation.CDN.Http
{
    public interface ISerializer
    {
        string Serialize<T>(T value);

        T Deserialize<T>(string value);
    }
}