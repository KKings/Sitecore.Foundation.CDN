namespace Sitecore.Foundation.CDN.Http
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Abstractions;

    public class HttpService : IHttpService
    {
        /// <summary>
        /// Sitecore Logging Implementation
        /// </summary>
        private readonly BaseLog logger;

        /// <summary>
        /// Serializer for converting objects to strings and strings to objects
        /// </summary>
        private readonly ISerializer serializer;

        public HttpService(BaseLog logger, ISerializer serializer)
        {
            this.logger = logger;
            this.serializer = serializer;
        }

        public virtual HttpWebRequest CreateWebRequest(Uri uri)
        {
            return (HttpWebRequest)WebRequest.Create(uri);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public virtual T Get<T>(Uri uri)
        {
            var httpWebRequest = this.CreateWebRequest(uri);

            httpWebRequest.Method = "GET";
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.Timeout = 30 * 1000;

            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                    // ReSharper disable once PossibleNullReferenceException
                using (var responseStream = response.GetResponseStream())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var value = new StreamReader(responseStream).ReadToEnd();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (WebException webException) // Anything other than a 200 series
            {
                this.logger.Error($"GET Request returned a non 200 status: {webException}", this);

                var response = (HttpWebResponse)webException.Response;

                using (var responseStream = response.GetResponseStream())
                {
                    var value = new StreamReader(responseStream).ReadToEnd();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (Exception exception)
            {
                this.logger.Error($"GET Request failed: {exception}", this);
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync<T>(Uri uri)
        {
            var httpWebRequest = this.CreateWebRequest(uri);

            httpWebRequest.Method = "GET";
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.Timeout = 30 * 1000;

            try
            {
                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                // ReSharper disable once PossibleNullReferenceException
                using (var responseStream = response.GetResponseStream())
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var value = await new StreamReader(responseStream).ReadToEndAsync();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (WebException webException) // Anything other than a 200 series
            {
                this.logger.Error($"GET Request returned a non 200 status: {webException}", this);

                var response = (HttpWebResponse)webException.Response;

                using (var responseStream = response.GetResponseStream())
                {
                    var value = await new StreamReader(responseStream).ReadToEndAsync();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (Exception exception)
            {
                this.logger.Error($"Async GET Request failed: {exception}", this);
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public virtual T Post<T>(Uri uri, object data, string contentType = "application/json")
        {
            var httpWebRequest = this.CreateWebRequest(uri);

            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 30 * 1000;
            httpWebRequest.ContentType = contentType;

            var s = data as string;
            var stringy = s ?? this.serializer.Serialize(data);

            var bytes = Encoding.UTF8.GetBytes(stringy);
            httpWebRequest.ContentLength = stringy.Length;

            try
            {
                var requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                using (var responseStream = response.GetResponseStream())
                {
                    var value = new StreamReader(responseStream).ReadToEnd();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (WebException webException) // Anything other than a 200 series
            {
                this.logger.Error($"POST Request returned a non 200 status: {webException}", this);

                var response = (HttpWebResponse)webException.Response;
                using (var responseStream = response.GetResponseStream())
                {
                    var value = new StreamReader(responseStream).ReadToEnd();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (Exception exception)
            {
                this.logger.Error($"POST Request Failed: {exception}", this);
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public virtual async Task<T> PostAsync<T>(Uri uri, object data, string contentType = "application/json")
        {
            var httpWebRequest = this.CreateWebRequest(uri);

            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 30 * 1000;
            httpWebRequest.ContentType = contentType;

            var s = data as string;
            var stringy = s ?? this.serializer.Serialize(data);

            var bytes = Encoding.UTF8.GetBytes(stringy);
            httpWebRequest.ContentLength = stringy.Length;

            try
            {
                var requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                using (var responseStream = response.GetResponseStream())
                {
                    var value = await new StreamReader(responseStream).ReadToEndAsync();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (WebException webException) // Anything other than a 200 series
            {
                this.logger.Error($"POST Request returned a non 200 status: {webException}", this);

                var response = (HttpWebResponse)webException.Response;
                using (var responseStream = response.GetResponseStream())
                {
                    var value = new StreamReader(responseStream).ReadToEnd();
                    return this.serializer.Deserialize<T>(value);
                }
            }
            catch (Exception exception)
            {
                this.logger.Error($"POST Request Failed: {exception}", this);
            }

            return default(T);
        }
    }
}