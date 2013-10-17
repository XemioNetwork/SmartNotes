using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Xemio.SmartNotes.Client.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="Byte"/> <see cref="Array"/> class.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Creates a bitmap image from the <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public static BitmapImage ToBitmapImage(this byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(bytes);
            image.EndInit();

            return image;
        }
    }
}
