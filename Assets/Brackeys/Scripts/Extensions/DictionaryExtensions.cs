using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtensions
{
    /// <summary>
    /// Modifies the dictionary, by adding all elements from the specified other dictionary into it.
    /// </summary>
    /// <remarks>
    /// Throws an error in the console on a conflict, but continues merging.
    /// This is useful to find all conflicts at once - but bugged behaviour is to be expected.
    /// </remarks>
    /// <param name="current">The base dictionary to modify.</param>
    /// <param name="other">The dictionary whose elements should be added to current dictionary.</param>
    /// <typeparam name="TKey">Type of key in dictionary.</typeparam>
    /// <typeparam name="TValue">Type of value in dictionary.</typeparam>
    /// <returns>A reference to the modified dictionary. NOT a new dictionary.</returns>
    public static Dictionary<TKey, TValue> MergeWith<TKey, TValue>(
        this Dictionary<TKey, TValue> current,
        Dictionary<TKey, TValue> other
    )
    {
        if (current == null && other == null)
        {
            return null;
        }

        if (current == null)
        {
            return new Dictionary<TKey, TValue>().MergeWith(other);
        }

        if (other == null)
        {
            return current;
        }

        foreach (var keyValuePair in other)
        {
            if (!current.TryAdd(keyValuePair.Key, keyValuePair.Value))
            {
                if (!current[keyValuePair.Key].Equals(other[keyValuePair.Key]))
                {
                    Debug.LogError(
                        $"Error when merging dictionaries - keys with conflicting values detected.\n"
                            + $" Conflicting key: {keyValuePair.Key}, conflicting values: current: {current[keyValuePair.Key]}, other:{other[keyValuePair.Key]}.\n"
                            + "\nContinuing with key from current."
                    );
                }
            }
        }

        return current;
    }
}
