using System;

/// <typeparam name="T"> type of Enum </typeparam>
public class EnumUtils<T>
    where T : struct, IConvertible
{
    /// <summary>
    /// Returns the number of cases in the enum.
    /// </summary>
    public static int Count
    {
        get
        {
            if (!typeof(T).IsEnum)
            {
                return -1;
            }

            return Enum.GetNames(typeof(T)).Length;
        }
    }

    /// <summary>
    /// Returns a random element from the enum.
    /// </summary>
    public static T RandomValue()
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }
        var enumValues = Enum.GetValues(typeof(T));
        return enumValues.RandomElement<T>();
    }
}
