using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomExtensions
{
    public static T ChooseRandom<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        var index = list.ChooseRandomIndex();

        return index >= 0 ? list[index] : default;
    }

    public static int ChooseRandomIndex<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        if (!list.Any())
            return -1;

        int count = list.Count;
        return Random.Range(0, count);
    }

}
