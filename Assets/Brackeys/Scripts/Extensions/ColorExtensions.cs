using UnityEngine;

public static class ColorExtensions
{
    /// <summary>
    /// Creates a new Color with the specified red component, keeping other components the same.
    /// </summary>
    public static Color WithR(this Color color, float newR)
    {
        return new Color(newR, color.g, color.b, color.a);
    }

    /// <summary>
    /// Creates a new Color with the specified green component, keeping other components the same.
    /// </summary>
    public static Color WithG(this Color color, float newG)
    {
        return new Color(color.r, newG, color.b, color.a);
    }

    /// <summary>
    /// Creates a new Color with the specified blue component, keeping other components the same.
    /// </summary>
    public static Color WithB(this Color color, float newB)
    {
        return new Color(color.r, color.g, newB, color.a);
    }

    /// <summary>
    /// Creates a new Color with the specified alpha component, keeping other components the same.
    /// </summary>
    public static Color WithA(this Color color, float newA)
    {
        return new Color(color.r, color.g, color.b, newA);
    }

    /// <summary>
    /// Creates a new Color with the specified alpha component, keeping other components the same.
    /// </summary>
    public static Color UniformRGB(this Color color, float newRGB)
    {
        return new Color(newRGB, newRGB, newRGB, color.a);
    }

    /// <summary>
    /// Creates a Color with the specified red component and full alpha.
    /// </summary>
    public static Color WithR(float newR)
    {
        return new Color(newR, 0, 0, 1);
    }

    /// <summary>
    /// Creates a Color with the specified green component and full alpha.
    /// </summary>
    public static Color WithG(float newG)
    {
        return new Color(0, newG, 0, 1);
    }

    /// <summary>
    /// Creates a Color with the specified blue component and full alpha.
    /// </summary>
    public static Color WithB(float newB)
    {
        return new Color(0, 0, newB, 1);
    }

    /// <summary>
    /// Creates a Color with only the specified alpha component.
    /// </summary>
    public static Color WithA(float newA)
    {
        return new Color(0, 0, 0, newA);
    }

    /// <summary>
    /// Creates a Color with the specified red component and zero alpha.
    /// </summary>
    public static Color WithRTransparent(float newR)
    {
        return new Color(newR, 0, 0, 0);
    }

    /// <summary>
    /// Creates a Color with the specified green component and zero alpha.
    /// </summary>
    public static Color WithGTransparent(float newG)
    {
        return new Color(0, newG, 0, 0);
    }

    /// <summary>
    /// Creates a Color with the specified blue component and zero alpha.
    /// </summary>
    public static Color WithBTransparent(float newB)
    {
        return new Color(0, 0, newB, 0);
    }

    /// <summary>
    /// Creates a new Color with all components set to the same value.
    /// </summary>
    public static Color Uniform(float uniformValue)
    {
        return new Color(uniformValue, uniformValue, uniformValue, uniformValue);
    }

    /// <summary>
    /// Creates a new Color with RGB components set to one value and alpha set to another.
    /// </summary>
    public static Color Uniform(float rgbValue, float alphaValue)
    {
        return new Color(rgbValue, rgbValue, rgbValue, alphaValue);
    }
}
