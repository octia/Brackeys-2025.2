using UnityEngine;

public static class Color32Extensions
{
    /// <summary>
    /// Converts a color to greyscale using the same coefficients as
    /// <seealso cref="Color"/> and rounds the result to nearest int.
    /// </summary>
    /// <returns>The current color as greyscale in range 0 to 255.</returns>
    public static int IntGreyscale(this Color32 color)
    {
        return Mathf.RoundToInt(Greyscale(color));
    }

    /// <summary>
    /// Converts a color to greyscale using the same coefficients as <seealso cref="Color"/>.
    /// </summary>
    /// <returns>The current color as greyscale in range 0 to 255.0.</returns>
    public static float Greyscale(this Color32 color)
    {
        return GreyscaleCoefficients.red * color.r
            + GreyscaleCoefficients.green * color.g
            + GreyscaleCoefficients.blue * color.b;
    }

    /// <summary>
    /// Converts a color to normalized greyscale using the same coefficients as
    /// <seealso cref="Color"/>.
    /// </summary>
    /// <returns>The current color as greyscale in range 0 to 1.0.</returns>
    public static float NormalizedGreyscale(this Color32 color)
    {
        return Greyscale(color) / 255.0f;
    }

    /// <summary>
    /// Creates a new Color32 with the specified red component, keeping other components the same.
    /// </summary>
    public static Color32 WithR(this Color32 color, byte newR)
    {
        return new Color32(newR, color.g, color.b, color.a);
    }

    /// <summary>
    /// Creates a new Color32 with the specified green component, keeping other components the same.
    /// </summary>
    public static Color32 WithG(this Color32 color, byte newG)
    {
        return new Color32(color.r, newG, color.b, color.a);
    }

    /// <summary>
    /// Creates a new Color32 with the specified blue component, keeping other components the same.
    /// </summary>
    public static Color32 WithB(this Color32 color, byte newB)
    {
        return new Color32(color.r, color.g, newB, color.a);
    }

    /// <summary>
    /// Creates a new Color32 with the specified alpha component, keeping other components the same.
    /// </summary>
    public static Color32 WithA(this Color32 color, byte newA)
    {
        return new Color32(color.r, color.g, color.b, newA);
    }

    /// <summary>
    /// Creates a Color32 with the specified red component and full alpha.
    /// </summary>
    public static Color32 WithR(byte newR)
    {
        return new Color32(newR, 0, 0, 255);
    }

    /// <summary>
    /// Creates a Color32 with the specified green component and full alpha.
    /// </summary>
    public static Color32 WithG(byte newG)
    {
        return new Color32(0, newG, 0, 255);
    }

    /// <summary>
    /// Creates a Color32 with the specified blue component and full alpha.
    /// </summary>
    public static Color32 WithB(byte newB)
    {
        return new Color32(0, 0, newB, 255);
    }

    /// <summary>
    /// Creates a Color32 with only the specified alpha component.
    /// </summary>
    public static Color32 WithA(byte newA)
    {
        return new Color32(0, 0, 0, newA);
    }

    /// <summary>
    /// Creates a Color32 with the specified red component and zero alpha.
    /// </summary>
    public static Color32 WithRTransparent(byte newR)
    {
        return new Color32(newR, 0, 0, 0);
    }

    /// <summary>
    /// Creates a Color32 with the specified green component and zero alpha.
    /// </summary>
    public static Color32 WithGTransparent(byte newG)
    {
        return new Color32(0, newG, 0, 0);
    }

    /// <summary>
    /// Creates a Color32 with the specified blue component and zero alpha.
    /// </summary>
    public static Color32 WithBTransparent(byte newB)
    {
        return new Color32(0, 0, newB, 0);
    }

    /// <summary>
    /// Creates a new Color32 with all components set to the same value.
    /// </summary>
    public static Color32 Uniform(byte uniformValue)
    {
        return new Color32(uniformValue, uniformValue, uniformValue, uniformValue);
    }

    /// <summary>
    /// Creates a new Color32 with RGB components set to one value and alpha set to another.
    /// </summary>
    public static Color32 Uniform(byte rgbValue, byte alphaValue)
    {
        return new Color32(rgbValue, rgbValue, rgbValue, alphaValue);
    }
}
