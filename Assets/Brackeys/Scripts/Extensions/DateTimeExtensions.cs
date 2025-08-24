using System;

public static class DateTimeExtensions
{
    /// <summary>
    /// Calculates the total number of days between two <see cref="DateTime"/> values.
    /// </summary>
    /// <param name="current">The current or reference date.</param>
    /// <param name="other">The date to compare with.</param>
    /// <param name="absolute">Determines whether the result should be an absolute value.</param>
    /// <returns>
    /// The total number of days between <paramref name="current"/> and <paramref name="other"/>.
    /// Optionally can provide the absolute value.
    /// </returns>
    /// <remarks>
    /// Useful when duration between days is needed.
    /// </remarks>
    public static double TotalDaysBetween(
        this DateTime current,
        DateTime other,
        bool absolute = true
    )
    {
        double totalDays = (current - other).TotalDays;
        return absolute ? Math.Abs(totalDays) : totalDays;
    }

    /// <summary>
    /// Calculates the total number of weeks between two <see cref="DateTime"/> values.
    /// </summary>
    /// <param name="current">The current or reference date.</param>
    /// <param name="other">The date to compare with.</param>
    /// <param name="absolute">Determines whether the result should be an absolute value.</param>
    /// <returns>
    /// The total number of weeks between <paramref name="current"/> and <paramref name="other"/>.
    /// Optionally can provide the absolute value.
    /// </returns>
    /// <remarks>
    /// Useful for computing week-based intervals.
    /// </remarks>
    public static double TotalWeeksBetween(
        this DateTime current,
        DateTime other,
        bool absolute = true
    ) => TotalDaysBetween(current, other, absolute) / TimeConstants.DaysPerWeek;
}

/// <summary>
/// Commonly used time unit constants for easy time calculations.
/// </summary>
/// <remarks>This does not account for daylight saving time changes.</remarks>
public static class TimeConstants
{
    public const int Second = 1;
    public const int SecondsPerMinute = 60 * Second;
    public const int SecondsPerHour = 60 * SecondsPerMinute;
    public const int SecondsPerDay = 24 * SecondsPerHour;

    public const int DaysPerWeek = 7;
    public const int SecondsPerWeek = DaysPerWeek * SecondsPerDay;
}
