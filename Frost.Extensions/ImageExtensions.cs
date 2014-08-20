using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Frost.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ToBytes(this Image image, ImageFormat format)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (format == null)
                throw new ArgumentNullException("format");

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, format);
                return stream.ToArray();
            }
        } 
    }
}