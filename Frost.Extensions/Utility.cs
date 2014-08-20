using System;
using System.Collections.Generic;

namespace Frost.Extensions
{
    internal class Utility
    {
        internal static void Swap<T>(ref T item1, ref T item2)
        {
            T dummy = item2;
            item2 = item1;
            item1 = dummy;
        }

        internal static void Heapify<T>(int heapIndex, T[] array, int count, IComparer<T> comparer, SortOrder order)
        {
            int firstdescendantIndex = 2*heapIndex + 1; // the first descendant 
            while (firstdescendantIndex < count)
            {

                switch (order)
                {
                    case SortOrder.Ascending:
                        if (firstdescendantIndex + 1 < count) // check for a second descendant
                            if (comparer.Compare(array[firstdescendantIndex + 1], array[firstdescendantIndex]) > 0)
                                firstdescendantIndex++;


                        if (comparer.Compare(array[heapIndex], array[firstdescendantIndex]) >= 0)
                            return; // the actual is heap so the job is done
                        // otherwise
                        Swap(ref array[heapIndex], ref array[firstdescendantIndex]);
                            // exchange firstdescendant and heap indexes
                        heapIndex = firstdescendantIndex; // continue
                        firstdescendantIndex = 2*heapIndex + 1;
                        break;
                    case SortOrder.Descending:
                        if (firstdescendantIndex + 1 < count) // check for a second descendant
                            if (comparer.Compare(array[firstdescendantIndex + 1], array[firstdescendantIndex]) < 0)
                                firstdescendantIndex++;


                        if (comparer.Compare(array[heapIndex], array[firstdescendantIndex]) <= 0)
                            return; // the actual is heap so the job is done
                        // otherwise
                        Swap(ref array[heapIndex], ref array[firstdescendantIndex]);
                            // exchange firstdescendant and heap indexes
                        heapIndex = firstdescendantIndex; // continue
                        firstdescendantIndex = 2*heapIndex + 1;
                        break;
                    default:
                        throw new ApplicationException("The sort order exception should be determined");

                }


            }
        }

    }
}