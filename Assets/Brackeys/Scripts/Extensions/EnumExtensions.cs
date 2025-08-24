public static class EnumExtensions
{
    /// <summary>
    /// Turns the enum case to camelCased string.
    /// Assumes the case name is in line with the style guide (Pascal case) and ToString isn't overriden.
    /// </summary>
    /// <param name="value">The enum to parse.</param>
    public static string ToCamelCase(this System.Enum value)
    {
        var name = value.ToString();
        return char.ToLower(name[0]) + name[1..];
    }
}
