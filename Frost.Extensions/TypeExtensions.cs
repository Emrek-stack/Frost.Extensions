using System;
using System.ComponentModel;

namespace Frost.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsBoolean(this Type type)
        {
            return type == typeof(Boolean);
        }

        public static bool IsString(this Type type)
        {
            return type == typeof(string);
        }

        public static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime);
        }

        public static bool IsNumeric(this Type t)
        {
            var type = t.GetTypeWithoutNullability();
            return
                type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(UInt16) ||
                type == typeof(UInt32) ||
                type == typeof(UInt64) ||
                type == typeof(decimal) ||
                type == typeof(float) ||
                type == typeof(double);
        }

        public static Type GetTypeWithoutNullability(this Type t)
        {
            return t.IsNullable() ? new NullableConverter(t).UnderlyingType : t;
        }

        public static bool IsNullable(this Type t)
        {
            return t.IsGenericType &&
                   t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsStatic(this Type t)
        {
            var c = t.GetConstructors();
            return (t.IsAbstract && t.IsSealed && c.Length == 0);
        }


    }
}