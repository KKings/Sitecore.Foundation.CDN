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

        /// <summary>
        /// Creates the Web Request
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <returns>Instance of <see cref="HttpWebRequest"/></returns>
        public virtual HttpWebRequest CreateWebRequest(Uri uri)
        {
            return (HttpWebRequest)WebRequest.Create(uri);
        }

        /// <summary>
        /// Synchronous GET to a <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <returns>Instance of <see cref="T"/></returns>
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
        /// Asynchronous GET to a <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <returns>Instance of <see cref="T"/></returns>
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
        /// Synchronous POST to a <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="data">The data to send in the stream</param>
        /// <param name="contentType">The content type of the stream</param>
        /// <returns>Instance of <see cref="T"/></returns>
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
        /// Asynchronous POST to a <see cref="Uri"/>
        /// </summary>
        /// <param name="uri">The Uri</param>
        /// <param name="data">The data to send in the stream</param>
        /// <param name="contentType">The content type of the stream</param>
        /// <returns>Instance of <see cref="T"/></returns>
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