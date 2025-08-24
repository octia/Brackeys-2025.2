using UnityEngine;

public static class Vector2Extensions
{
    /// <summary>
    /// Returns a new Vector2 where both the x and y components are halved from the original.
    /// </summary>
    /// <param name="value">The original Vector2 to be halved.</param>
    /// <returns>A new Vector2 with both components divided by 2.</returns>
    public static Vector2 Half(this Vector2 value) => value * 0.5f;

    /// <summary>
    /// Creates a new Vector2 with the specified x component, keeping the y component the same.
    /// </summary>
    public static Vector2 WithX(this Vector2 vector, float newX)
    {
        return new Vector2(newX, vector.y);
    }

    /// <summary>
    /// Creates a new Vector2 with the specified y component, keeping the x component the same.
    /// </summary>
    public static Vector2 WithY(this Vector2 vector, float newY)
    {
        return new Vector2(vector.x, newY);
    }

    /// <summary>
    /// Creates a Vector2 with the specified x component and y component set to zero.
    /// </summary>
    public static Vector2 WithX(float newX)
    {
        return new Vector2(newX, 0);
    }

    /// <summary>
    /// Creates a Vector2 with the specified y component and x component set to zero.
    /// </summary>
    public static Vector2 WithY(float newY)
    {
        return new Vector2(0, newY);
    }

    /// <summary>
    /// Clamps the absolute value of each component to be at most maxValue.
    /// </summary>
    public static Vector2 ComponentWiseClamp(this Vector2 vector, float maxValue)
    {
        vector = Vector2.Min(vector, Vector2.one * maxValue);
        vector = Vector2.Max(vector, -Vector2.one * maxValue);
        return vector;
    }
}
