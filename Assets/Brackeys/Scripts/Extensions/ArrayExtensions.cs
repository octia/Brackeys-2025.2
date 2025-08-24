using System;

public static class ArrayExtensions
{
    /// <summary>
    /// Returns a random element from an array.
    /// </summary>
    public static T RandomElement<T>(this Array inputArray)
    {
        if (inputArray.Length == 0)
        {
            throw new IndexOutOfRangeException("No elements to return from the inputArray.");
        }
        int index = UnityEngine.Random.Range(0, inputArray.Length);
        return (T)inputArray.GetValue(index);
    }
}
