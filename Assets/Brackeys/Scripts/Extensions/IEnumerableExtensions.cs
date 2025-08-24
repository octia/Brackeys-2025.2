using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    /// <summary>
    /// Returns a random element from an enumerable.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable</typeparam>
    /// <param name="input">The enumerable to return the element from.</param>
    public static T RandomElement<T>(this IEnumerable<T> input)
    {
        int count = input.Count();
        if (count == 0)
        {
            throw new IndexOutOfRangeException("No elements to return from the input enumerable.");
        }
        return input.ElementAt(UnityEngine.Random.Range(0, count));
    }

    /// <summary>
    /// Returns whether the enumerable is null, or empty.
    /// </summary>
    /// <returns>True if the enumerable is null, or if it has 0 elements.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> input)
    {
        if (input == null)
        {
            return true;
        }
        return !input.Any();
    }
}
