using System;
using System.Collections.Generic;
using System.Linq;

public static class HashsetExtensions
{
    /// <summary>
    /// Compares two sets to check if they contain the same elements, regardless of order.
    /// </summary>
    public static bool SetsAreEqual<T, TKey>(
        this HashSet<T> set1,
        HashSet<T> set2,
        Func<T, TKey> keySelector
    )
    {
        if (set1 == null && set2 == null)
        {
            return true;
        }
        if (set1 == null || set2 == null)
        {
            return false;
        }

        var set1Keys = set1.Select(keySelector).ToHashSet();
        var set2Keys = set2.Select(keySelector).ToHashSet();

        return set1Keys.SetEquals(set2Keys);
    }

    /// <summary>
    /// Generates a hash code for a set of strings to include in HashCode.Combine.
    /// </summary>
    public static int SetHashCode<T, TKey>(this HashSet<T> set, Func<T, TKey> keySelector)
    {
        if (set == null || set.Count == 0)
        {
            return 0;
        }

        int hash = 0;
        foreach (var item in set.Select(keySelector))
        {
            hash = HashCode.Combine(hash, item?.GetHashCode() ?? 0);
        }

        return hash;
    }

    /// <summary>
    /// Cleans a set of elements by removing empty or whitespace-only entries.
    /// </summary>
    public static HashSet<string> GetCleanedUpSet(this HashSet<string> setToClean)
    {
        return setToClean.Where(c => !string.IsNullOrWhiteSpace(c)).ToHashSet();
    }
}
