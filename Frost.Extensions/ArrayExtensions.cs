using System;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Extensions
{
    public static class ArrayExtensions
    {
        public static List<T> ToList<T>(this Array items, Func<object, T> mapFunction)
        {
            if (items == null || mapFunction == null)
                return new List<T>();
            return items.Cast<object>().Select((t, i) => mapFunction(items.GetValue(i))).Where(val => val != null).ToList();
        }
    }
}