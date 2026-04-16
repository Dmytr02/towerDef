using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExternalFunctions
{
    public static void AddSorted<T>(this List<T> list, T item) where T : IComparable<T>
    {
        int index = list.BinarySearch(item);
        
        if (index < 0) 
            index = ~index;

        list.Insert(index, item);
    }
}
