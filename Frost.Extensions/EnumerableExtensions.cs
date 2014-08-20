using System;
using System.Collections.Generic;
using System.Linq;

namespace Frost.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> target)
        {
            Random r = new Random();
            return target.OrderBy(x => (r.Next()));
        }

        public static IEnumerable<T> RemoveDuplicates<T>(this ICollection<T> list, Func<T, int> predicate)
        {
            var dict = new Dictionary<int, T>();

            foreach (var item in list)
            {
                if (!dict.ContainsKey(predicate(item)))
                {
                    dict.Add(predicate(item), item);
                }
            }

            return dict.Values.AsEnumerable();
        }

        public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> groupings)
        {
            return groupings.ToDictionary(group => group.Key, group => group.ToList());
        }

        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
        {
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

            var l = source.ToLookup(firstKeySelector);
            foreach (var item in l)
            {
                var dict = new Dictionary<TSecondKey, TValue>();
                retVal.Add(item.Key, dict);
                var subdict = item.ToLookup(secondKeySelector);
                foreach (var subitem in subdict)
                {
                    dict.Add(subitem.Key, aggregate(subitem));
                }
            }

            return retVal;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            var r = new Random((int)DateTime.Now.Ticks);
            var shuffledList = list.Select(x => new { Number = r.Next(), Item = x }).OrderBy(x => x.Number).Select(x => x.Item);
            return shuffledList.ToList();
        }

         #region exchange sorts
        static public IEnumerable<T> SortBubble<T>(this IEnumerable<T> items, IComparer<T> comparer,SortOrder order)
        {
            T[] array = Enumerable.ToArray(items);

            int count = array.Count();
            do
            {
                for (int i = 0; i < count - 1; i++)
                {
                    switch (order)
                    {
                        case SortOrder.Descending:
                            if (comparer.Compare(array[i], array[i + 1]) < 0)
                            {
                                Utility.Swap(ref array[i], ref array[i + 1]);
                            } 
                            break;
                        case SortOrder.Ascending:
                            if (comparer.Compare(array[i], array[i + 1]) > 0)
                            {
                                Utility.Swap(ref array[i], ref array[i + 1]);
                            } 
                            break;
                        default:
                            throw new ApplicationException("Order sould be precised");
                    }
                }
                count--;

            } while (count > 1);
            
            foreach (var item in array)
            {
                yield return item;
            }
        }
   
        static public IEnumerable<T> SortCocktail<T>(this IEnumerable<T> items, IComparer<T> comparer, SortOrder order) 
        {
            T[] array = Enumerable.ToArray(items);
            int count = array.Count();
            bool flag = false;

            switch (order)
            {
                case SortOrder.Ascending:
                    do
                    {
                        flag = false;

                        for (int i = 0; i < count - 2; i++)
                        {
                            if (comparer.Compare(array[i], array[i + 1]) > 0)
                            {
                                Utility.Swap(ref array[i], ref array[i + 1]);
                                flag = true;
                            }
                        }
                        if (flag == false)
                        {
                            break;
                        }
                        flag = false;

                        for (int i = count - 2; i > 0; i--)
                        {
                            if (comparer.Compare(array[i], array[i + 1]) > 0)
                            {
                                Utility.Swap(ref array[i], ref array[i + 1]);
                                flag = true;
                            }
                        }

                    } while (flag == true);
                    break;
                    case SortOrder.Descending:
                    do
                    {
                        flag = false;

                        for (int i = 0; i < count - 2; i++)
                        {
                            if (comparer.Compare(array[i], array[i + 1]) < 0)
                            {
                                Utility.Swap(ref array[i], ref array[i + 1]);
                                flag = true;
                            }
                        }
                        if (flag == false)
                        {
                            break;
                        }
                        flag = false;

                        for (int i = count - 2; i > 0; i--)
                        {
                            if (comparer.Compare(array[i], array[i + 1]) < 0)
                            {
                                Utility.Swap(ref array[i], ref array[i + 1]);
                                flag = true;
                            }
                        }

                    } while (flag == true);
                    break;
                default:
                    throw new ApplicationException("The sort order exception should be determined");
            }


            foreach (var item in array)
            {
                yield return item;
            }
        }

        static public IEnumerable<T> SortEvenOdd<T>(this IEnumerable<T> items, IComparer<T> comparer, SortOrder order)
        {

            T[] array = Enumerable.ToArray(items);
            int count = array.Count();
            int Max = (count % 2 == 0) ? 2 * (count / 2) - 1 : 2 * (count - 1) / 2;

            switch (order)
            {
                case SortOrder.Ascending:
                    for (int i = 0; i < count / 2; i++)
                    {
                        for (int j = 0; j < Max; j++)
                        {
                            if (comparer.Compare(array[j], array[j + 1]) > 0)
                            {
                                Utility.Swap(ref array[j], ref array[j + 1]);
                            }
                        }
                        for (int j = 1; j < Max; j++)
                        {
                            if (comparer.Compare(array[j], array[j + 1]) > 0)
                            {
                                Utility.Swap(ref array[j], ref array[j + 1]);
                            }
                        }
                    }
                    break;
                case SortOrder.Descending:
                    for (int i = 0; i < count / 2; i++)
                    {
                        for (int j = 0; j < Max; j++)
                        {
                            if (comparer.Compare(array[j], array[j + 1]) < 0)
                            {
                                Utility.Swap(ref array[j], ref array[j + 1]);
                            }
                        }
                        for (int j = 1; j < Max; j++)
                        {
                            if (comparer.Compare(array[j], array[j + 1]) < 0)
                            {
                                Utility.Swap(ref array[j], ref array[j + 1]);
                            }
                        }
                    }
                    break;
                default:
                    throw new ApplicationException("The sort order exception should be determined");
            }
            
            
            
            for (int i = 0; i < count / 2; i++)
            {
                for (int j = 0; j < Max; j++)
                {
                    if (comparer.Compare(array[j] , array[j + 1])>0)
                    {
                        Utility.Swap(ref array[j], ref array[j + 1]);
                    }
                }
                for (int j = 1; j < Max; j++)
                {
                    if (comparer.Compare(array[j], array[j + 1]) > 0)
                    {
                        Utility.Swap(ref array[j], ref array[j + 1]);
                    }
                }
            }

            foreach (var item in array)
            {
                yield return item;
            }
        }

        static public IEnumerable<T> SortComb<T>(this IEnumerable<T> items, IComparer<T> comparer, SortOrder order)
        {
            T[] array = Enumerable.ToArray(items);
            int count = array.Count();

            int gap = count;
            bool swapped = true;

            switch (order)
            {
                case SortOrder.Ascending:
                    while (gap > 1 || swapped)
                    {
                        if (gap > 1)
                            gap = (int)(gap / 1.247330950103979);

                        int i = 0;
                        swapped = false;
                        while (i + gap < count)
                        {
                            if (comparer.Compare(array[i], array[i + gap]) > 0)
                            {
                                Utility.Swap(ref array[i], ref  array[i + gap]);
                                swapped = true;
                            }
                            i++;
                        }
                    }
                    break;
                case SortOrder.Descending:
                    while (gap > 1 || swapped)
                    {
                        if (gap > 1)
                            gap = (int)(gap / 1.247330950103979);

                        int i = 0;
                        swapped = false;
                        while (i + gap < count)
                        {
                            if (comparer.Compare(array[i], array[i + gap]) < 0)
                            {
                                Utility.Swap(ref array[i], ref  array[i + gap]);
                                swapped = true;
                            }
                            i++;
                        }
                    }
                    break;
                default:
                    throw new ApplicationException("The sort order exception should be determined");
            }
            
            
            
            while (gap > 1 || swapped)
            {
                if (gap > 1)
                    gap = (int)(gap / 1.247330950103979);

                int i = 0;
                swapped = false;
                while (i + gap < count)
                {
                    if (comparer.Compare(array[i], array[i + gap]) > 0)
                    {
                        Utility.Swap(ref array[i], ref  array[i + gap]);
                        swapped = true;
                    }
                    i++;
                }
            }
            
            foreach (var item in array)
            {
                yield return item;
            }
        }

        static public IEnumerable<T> SortGenome<T>(this IEnumerable<T> items,IComparer<T> comparer, SortOrder order)
        {

            T[] array = Enumerable.ToArray(items);
            int count = array.Count();


            int position = 0;


            switch (order)
            {
                case SortOrder.Ascending:
                    position = 0;
                    while (position < count)
                    {
                        if (position == 0 || comparer.Compare(array[position] , array[position - 1])>0)
                        {
                            position++;
                        }
                        else
                        {
                            Utility.Swap(ref array[position], ref array[position - 1]);
                            position = position - 1;
                        }
                    }
                    break;
                case SortOrder.Descending:
                    position = 0;
                    while (position < count)
                    {
                        if (position == 0 || comparer.Compare(array[position] , array[position - 1])<0)
                        {
                            position++;
                        }
                        else
                        {
                            Utility.Swap(ref array[position], ref array[position - 1]);
                            position = position - 1;
                        }
                    }
                    break;
                default:
                    throw new ApplicationException("The sort order exception should be determined");
            }
             

            
            
            foreach (var item in array)
            {
                yield return item;
            }
        }
        
        #endregion


        #region selection sorts

        static public IEnumerable<T> SortSelection<T>(this IEnumerable<T> items, IComparer<T> comparer, SortOrder order)
        {
            T[] array = Enumerable.ToArray(items);
            int count = array.Count();


            int minIndex;
            T minValue;
            for (int i = 0; i < array.Length - 1; i++)
            {
                minIndex = i;
                minValue = array[i];
                for (int j = i + 1; j < array.Length; j++)
                {
                    //The sort order

                    switch (order)
                    {
                        case SortOrder.Descending:
                        if (comparer.Compare(array[j],minValue)>0)
                        {
                            minIndex = j;
                            minValue = array[j];
                            array[minIndex] = array[i];
                            array[i] = minValue;

                        }
                            break;
                        case SortOrder.Ascending:
                            if (comparer.Compare(array[j], minValue) <= 0)
                            {
                                minIndex = j;
                                minValue = array[j];
                                array[minIndex] = array[i];
                                array[i] = minValue;

                            }
                            break;
                        default:
                            throw new ApplicationException("The sort order exception should be determined");
                    }

                    
                }
            }

            foreach (var item in array)
            {
                yield return item;
            }
        }

         static public IEnumerable<T> SortHeap<T>(this IEnumerable<T> items, IComparer<T> comparer, SortOrder order)
         {
             
            T[] array = Enumerable.ToArray(items);

            int count = array.Length;
            for (int index = count / 2 - 1; index >= 0; index--)
                Utility.Heapify(index, array, count,comparer,order);

            while (count > 1)
            {
                count--;
                Utility.Swap(ref array[0], ref array[count]);
                Utility.Heapify(0, array, count,comparer,order);
            }

            foreach (var item in array)
            {
                yield return item;
            }
         }
         
         
        #endregion

    }
}