using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="Image"/> class.
    /// </summary>
    public static class ImageExtensions
    {


        public static Stream ToPngStream(this Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);

            return stream;
        }

        /// <summary>
        /// Resizes the given image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            if (width == 0 || height == 0)
                return new Bitmap(image);

            Bitmap resizedBitmap = new Bitmap(width, height);
            resizedBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(image, 0, 0, resizedBitmap.Width, resizedBitmap.Height);
            }

            return resizedBitmap;
        }
    }
}
