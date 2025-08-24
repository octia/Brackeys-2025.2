using System;

public static class RandomExtensions
{
    public static int NextIntInRange(this Random random, Range range)
    {
        return random.Next(range.Start.Value, range.End.Value);
    }

    public static double NextDoubleInRange(this Random random, Range range)
    {
        var end = (double)range.End.Value;
        var start = (double)range.Start.Value;
        var rangeLength = end - start;

        if (rangeLength < 0.001f)
        {
            return 0;
        }

        var raw = random.NextDouble();
        return raw * rangeLength + start;
    }

    public static float NextFloatInRange(this Random random, Range range)
    {
        return (float)NextDoubleInRange(random, range);
    }
}
