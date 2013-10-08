using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xemio.SmartNotes.Client.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="HttpClient"/> class.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Posts the json async.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="json">The json.</param>
        public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient client, string requestUri, string json)
        {
            return client.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));
        }
        /// <summary>
        /// Puts the json async.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="json">The json.</param>
        public static Task<HttpResponseMessage> PutJsonAsync(this HttpClient client, string requestUri, string json)
        {
            return client.PutAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));
        }
    }
}
