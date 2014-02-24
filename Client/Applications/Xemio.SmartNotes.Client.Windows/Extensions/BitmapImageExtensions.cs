using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Xemio.SmartNotes.Client.Windows.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="BitmapImage"/> class.
    /// </summary>
    public static class BitmapImageExtensions
    {
        /// <summary>
        /// Creates a <see cref="MemoryStream"/> containing the image as PNG.
        /// </summary>
        /// <param name="image">The image.</param>
        public static MemoryStream ToPngMemoryStream(this BitmapImage image)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            var stream = new MemoryStream();
            encoder.Save(stream);

            return stream;
        }
    }
}
