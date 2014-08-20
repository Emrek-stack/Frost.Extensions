using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;

namespace Frost.Extensions
{
    public static class ByteArrayExtensions
    {
        public static Image ToImage(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            return Image.FromStream(new MemoryStream(bytes));
        }

        public static T ToObject<T>(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = binForm.Deserialize(memStream);
            return (T)obj;
        }
    }
}