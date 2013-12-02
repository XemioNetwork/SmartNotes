using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="HttpRequestMessage"/> class.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Creates an <see cref="HttpResponseMessage"/> containing a image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="imageStream">The image stream.</param>
        public static HttpResponseMessage CreateImageStreamResponse(this HttpRequestMessage request, Stream imageStream)
        {
            imageStream.Seek(0, SeekOrigin.Begin);

            var response = new HttpResponseMessage(HttpStatusCode.Found)
                               {
                                   Content = new StreamContent(imageStream)
                               };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("images/png");

            return response;
        }
    }
}
