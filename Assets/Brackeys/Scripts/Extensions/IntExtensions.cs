using System;
using System.Globalization;

public static class IFormattableExtensions
{
    public static string InvariantString(this IFormattable value)
    {
        return value.ToString("", CultureInfo.InvariantCulture);
    }
}
