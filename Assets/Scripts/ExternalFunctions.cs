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

    public static Vector3 Multiply(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }public static Vector3 Devide(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }
}
