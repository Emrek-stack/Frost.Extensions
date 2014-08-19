using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Frost.Extensions
{
    public static class GenericExtensions
    {
        public static T DeepClone<T>(this T input) where T : ISerializable
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, input);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T2 GetValueOrDefault<T1, T2>(this T1 prop1, Func<T1, T2> prop2)
        {
            return prop1 != null ? prop2(prop1) : default(T2);
        }

        public static T3 GetValueOrDefault<T1, T2, T3>(this T1 prop1, Func<T1, T2> prop2, Func<T2, T3> prop3)
        {
            var prop = prop1.GetValueOrDefault(prop2);
            return Comparer<T2>.Default.Compare(prop, default(T2)) != 0 ? prop3(prop) : default(T3);
        }

        public static T4 GetValueOrDefault<T1, T2, T3, T4>(this T1 prop1, Func<T1, T2> prop2, Func<T2, T3> prop3, Func<T3, T4> prop4)
        {
            var prop = prop1.GetValueOrDefault(prop2, prop3);
            return Comparer<T3>.Default.Compare(prop, default(T3)) != 0 ? prop4(prop) : default(T4);
        }

        public static T5 GetValueOrDefault<T1, T2, T3, T4, T5>(this T1 prop1, Func<T1, T2> prop2, Func<T2, T3> prop3, Func<T3, T4> prop4, Func<T4, T5> prop5)
        {
            var prop = prop1.GetValueOrDefault(prop2, prop3, prop4);
            return Comparer<T4>.Default.Compare(prop, default(T4)) != 0 ? prop5(prop) : default(T5);
        }

        public static T6 GetValueOrDefault<T1, T2, T3, T4, T5, T6>(this T1 prop1, Func<T1, T2> prop2, Func<T2, T3> prop3, Func<T3, T4> prop4, Func<T4, T5> prop5, Func<T5, T6> prop6)
        {
            var prop = prop1.GetValueOrDefault(prop2, prop3, prop4, prop5);
            return Comparer<T5>.Default.Compare(prop, default(T5)) != 0 ? prop6(prop) : default(T6);
        }

        public static T7 GetValueOrDefault<T1, T2, T3, T4, T5, T6, T7>(this T1 prop1, Func<T1, T2> prop2, Func<T2, T3> prop3, Func<T3, T4> prop4, Func<T4, T5> prop5, Func<T5, T6> prop6, Func<T6, T7> prop7)
        {
            var prop = prop1.GetValueOrDefault(prop2, prop3, prop4, prop5, prop6);
            return Comparer<T6>.Default.Compare(prop, default(T6)) != 0 ? prop7(prop) : default(T7);
        }
    }
}