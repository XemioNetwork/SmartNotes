using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xemio.SmartNotes.Client.Shared.Extensions
{
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Reads the content as the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="content">The content.</param>
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            string response = await content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<T>(response);
        }
    }
}
