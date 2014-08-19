using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Frost.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
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