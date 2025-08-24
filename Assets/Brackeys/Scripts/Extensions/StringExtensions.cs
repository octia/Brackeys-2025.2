using System.Globalization;
using System.Text;

public static class StringExtensions
{
    public static string Repeat(this string s, int n) =>
        new StringBuilder(s.Length * n).Insert(0, s, n).ToString();

    /// <summary>
    /// Converts a string representation into a boolean value.
    /// Accepts "yes", "true", or any nonzero integer as true, and everything else as false.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>Returns true for "yes", "true", or any nonzero integer. Otherwise, returns false.</returns>
    public static bool ParseBool(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        value = value.Trim().ToLower();

        if (int.TryParse(value, out int intValue))
        {
            return intValue != 0;
        }

        return value == "yes" || value == "true";
    }

    /// <summary>
    /// Parses a string value into an integer.
    /// Returns 0 if the value is null, empty, or not a valid number.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>The parsed integer, or 0 if parsing fails.</returns>
    public static int ParseInt(this string value) =>
        int.TryParse(value, out int result) ? result : 0;

    /// <summary>
    /// Capitalizes the first letter of the given string using the specified culture.
    /// This ensures correct casing behavior across different locales (e.g. Turkish 'i' → 'İ').
    /// </summary>
    /// <param name="input">The string to modify.</param>
    /// <param name="culture">The culture used to determine proper casing rules.</param>
    /// <returns>The string with the first letter capitalized, or the original string if empty or null.</returns>
    public static string CapitalizeFirstLetter(this string input, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToUpper(input[0], culture) + input.Substring(1);
    }
}
