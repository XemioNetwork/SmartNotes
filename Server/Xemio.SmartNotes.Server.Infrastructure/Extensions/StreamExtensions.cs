using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Creates a byte array with the content of the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static byte[] ToByteArray(this Stream stream)
        {
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}