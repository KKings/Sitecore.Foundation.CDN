namespace Sitecore.Foundation.CDN.Http
{
    using System;
    using System.Threading.Tasks;

    public interface IHttpService
    {
        T Get<T>(Uri uri);

        Task<T> GetAsync<T>(Uri uri);

        T Post<T>(Uri uri, object data, string contentType);

        Task<T> PostAsync<T>(Uri uri, object data, string contentType);
    }
}