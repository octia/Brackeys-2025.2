using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
    /// <summary>
    /// Returns a random element from the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inputList"></param>
    /// <param name="random">The random number generator to use.</param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public static T RandomElement<T>(this List<T> inputList)
    {
        if (inputList.IsNullOrEmpty())
        {
            throw new IndexOutOfRangeException("No elements to return from the inputList.");
        }

        int index = UnityEngine.Random.Range(0, inputList.Count);
        return inputList[index];
    }

    /// <summary>
    /// Returns a random index of an element within the <paramref name="inputList"/> range that
    /// is different from <paramref name="originalIndex"/>.
    /// </summary>
    /// <returns>-1 if the original item is not on the <paramref name="inputList"/>.</returns>
    /// <throws>IndexOutOfRangeException if the list doesn't have at least 2 elements.</throws>
    public static int RandomIndexDifferentFrom<T>(this List<T> inputList, int originalIndex)
    {
        if (inputList.IsNullOrEmpty())
        {
            throw new IndexOutOfRangeException("No elements to return from the inputList.");
        }

        if (inputList.Count < 2)
        {
            throw new IndexOutOfRangeException("Only one element on the list.");
        }

        if (originalIndex < 0 || originalIndex >= inputList.Count)
        {
            Debug.LogWarning("Original index outside of the list bounds.");
            // If the original item wasn't on the list, any new index will be valid.
            return UnityEngine.Random.Range(0, maxExclusive: inputList.Count);
        }

        // Get the second index from the shorter array to skip originalIndex.
        int newIndex = UnityEngine.Random.Range(0, inputList.Count - 1);

        if (newIndex >= originalIndex)
        {
            // If the newIndex is bigger or equal to originalIndex, increase it to effectively
            // skip originalIndex in the array indices.
            newIndex++;
        }

        return newIndex;
    }

    /// <summary>
    /// Returns a random index of an element within the <paramref name="inputList"/> range that
    /// is different from the index of <paramref name="originalItem"/>.
    /// </summary>
    /// <returns>-1 if the original item is not on the <paramref name="inputList"/>.</returns>
    /// <throws>IndexOutOfRangeException if the list doesn't have at least 2 elements.</throws>
    public static int RandomIndexDifferentFrom<T>(
        this List<T> inputList,
        Predicate<T> originalMatch
    )
    {
        if (inputList.IsNullOrEmpty())
        {
            throw new IndexOutOfRangeException("No elements to return from the inputList.");
        }

        int originalIndex = inputList.FindIndex(originalMatch);
        if (originalIndex == -1)
        {
            Debug.LogWarning("Original item not on the list.");
            // If the original item wasn't on the list, any new index will be valid.
            return UnityEngine.Random.Range(0, inputList.Count);
        }

        return inputList.RandomIndexDifferentFrom(originalIndex);
    }

    /// <summary>
    /// Swap two elements on the list.
    /// </summary>
    public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        if (indexA < 0 || indexB < 0)
        {
            throw new IndexOutOfRangeException($"Negative index given: {indexA} {indexB}.");
        }
        if (indexA > list.Count || indexB > list.Count)
        {
            throw new IndexOutOfRangeException(
                $"Index outside of the bounds of the list: passed {indexA} and {indexB}  - "
                    + $"list has {list.Count} elements."
            );
        }
        (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
        return list;
    }

    /// <summary>
    /// Compares two lists to ensure they contain the same elements, regardless of order.
    /// </summary>
    public static bool ListsAreEqual(this List<string> list1, List<string> list2)
    {
        if (list1 == null && list2 == null)
        {
            return true;
        }
        if (list1 == null || list2 == null)
        {
            return false;
        }

        return list1.Count == list2.Count && !list1.Except(list2).Any();
    }

    /// <summary>
    /// Compares two lists to ensure they contain the same elements, regardless of order.
    /// Optionally compares elements by a selected key instead of the objects themselves.
    /// </summary>
    /// <param name="list1">The first list to compare.</param>
    /// <param name="list2">The second list to compare.</param>
    /// <param name="keySelector">
    /// An optional function that selects a comparison key from each element.
    /// If null, elements are compared directly using their default equality.
    /// </param>
    /// <returns>True if the lists contain the same elements (by value or key), otherwise false.</returns>
    public static bool ListsAreEqual<T, TKey>(
        this IList<T> list1,
        IList<T> list2,
        Func<T, TKey> keySelector = null
    )
    {
        if (list1 == null && list2 == null)
        {
            return true;
        }
        if (list1 == null || list2 == null)
        {
            return false;
        }
        if (list1.Count != list2.Count)
        {
            return false;
        }

        if (keySelector == null)
        {
            return new HashSet<T>(list1).SetEquals(list2);
        }

        var set1 = list1.Select(keySelector).ToHashSet();
        var set2 = list2.Select(keySelector).ToHashSet();
        return set1.SetEquals(set2);
    }

    /// <summary>
    /// Generates a combined hash code for a list based on a key selector.
    /// Elements are treated as a set (order-insensitive).
    /// </summary>
    /// <param name="list">The list to hash.</param>
    /// <param name="keySelector">Selector used to extract a unique value for hashing.</param>
    /// <returns>A combined hash code for the list contents.</returns>
    public static int SetHashCode<T, TKey>(this List<T> list, Func<T, TKey> keySelector)
    {
        if (list == null || list.Count == 0)
        {
            return 0;
        }

        int hash = 0;
        var seenKeys = new HashSet<TKey>();

        foreach (var item in list)
        {
            if (item == null)
            {
                continue;
            }

            var key = keySelector(item);

            if (key == null || !seenKeys.Add(key))
            {
                continue;
            }

            hash = HashCode.Combine(hash, key.GetHashCode());
        }

        return hash;
    }

    /// <summary>
    /// Returns the last item from the list for which the provided condition (valueSelector <= currentValue) is met.
    /// </summary>
    /// <typeparam name="T">Type of the elements.</typeparam>
    /// <param name="list">List to search.</param>
    /// <param name="currentValue">The current reference value to compare against.</param>
    /// <param name="valueSelector">Function that returns the comparable value for each item.</param>
    /// <returns>The last matching item or null if none match.</returns>
    /// <remarks>
    /// Assumes list is sorted in ascending order.
    /// </remarks>
    public static T GetLastLessThanOrEqual<T>(
        this List<T> list,
        int currentValue,
        Func<T, int> valueSelector
    )
        where T : class
    {
        T last = null;

        foreach (var item in list)
        {
            if (currentValue >= valueSelector(item))
            {
                last = item;
            }
            else
            {
                break;
            }
        }

        return last;
    }

    /// <summary>
    /// Returns the first element in the list whose selected value is strictly greater than the given threshold.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to search through.</param>
    /// <param name="currentValue">The reference value to compare against.</param>
    /// <param name="valueSelector">Function used to extract a comparable value from each list element.</param>
    /// <returns>The first element satisfying the condition, or null if none do.</returns>
    /// <remarks>
    /// Assumes list is sorted in ascending order.
    /// </remarks>
    public static T GetFirstGreaterThan<T>(
        this List<T> list,
        int currentValue,
        Func<T, int> valueSelector
    )
        where T : class
    {
        foreach (var item in list)
        {
            if (valueSelector(item) > currentValue)
            {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the last item in a sorted list where the selected value falls within the specified range.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="list">The list to search (assumed sorted in ascending order).</param>
    /// <param name="from">Lower bound of the range.</param>
    /// <param name="to">Upper bound of the range.</param>
    /// <param name="selector">Function to select an integer value from each list item.</param>
    /// <param name="includeFrom">If true, the lower bound is inclusive (value ≥ from); otherwise exclusive (value &gt; from).</param>
    /// <param name="includeTo">If true, the upper bound is inclusive (value ≤ to); otherwise exclusive (value &lt; to).</param>
    /// <returns>The last item in the list that matches the specified range, or null if none found.</returns>
    public static T GetLastBetween<T>(
        this List<T> list,
        int from,
        int to,
        Func<T, int> selector,
        bool includeFrom = true,
        bool includeTo = false
    )
        where T : class
    {
        T result = null;

        foreach (var item in list)
        {
            var value = selector(item);

            bool withinLower = includeFrom ? value >= from : value > from;
            bool withinUpper = includeTo ? value <= to : value < to;

            if (withinLower && withinUpper)
            {
                result = item;
            }
        }

        return result;
    }
}
