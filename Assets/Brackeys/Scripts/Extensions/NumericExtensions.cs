public static class NumericExtensions
{
    /// <summary>
    /// Returns half the value of the specified floating-point number.
    /// </summary>
    /// <param name="value">The floating-point number to halve.</param>
    /// <returns>The halved value of the input floating-point number.</returns>
    public static float Half(this float value)
    {
        return value * 0.5f;
    }

    /// <summary>
    /// Returns half the value of the specified floating-point number.
    /// </summary>
    /// <param name="value">The floating-point number to halve.</param>
    /// <returns>The halved value of the input floating-point number.</returns>
    public static double Half(this double value)
    {
        return value * 0.5;
    }
}
