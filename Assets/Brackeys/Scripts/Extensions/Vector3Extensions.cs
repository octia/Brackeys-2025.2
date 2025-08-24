using UnityEngine;

public static class Vector3Extensions
{
    /// <summary>
    /// Creates a new Vector3 with the specified x component, keeping the y and z components the same.
    /// </summary>
    public static Vector3 WithX(this Vector3 vector, float newX)
    {
        return new Vector3(newX, vector.y, vector.z);
    }

    /// <summary>
    /// Creates a new Vector3 with the specified y component, keeping the x and z components the same.
    /// </summary>
    public static Vector3 WithY(this Vector3 vector, float newY)
    {
        return new Vector3(vector.x, newY, vector.z);
    }

    /// <summary>
    /// Creates a new Vector3 with the specified z component, keeping the x and y components the same.
    /// </summary>
    public static Vector3 WithZ(this Vector3 vector, float newZ)
    {
        return new Vector3(vector.x, vector.y, newZ);
    }

    /// <summary>
    /// Creates a Vector3 with the specified x component and y and z components set to zero.
    /// </summary>
    public static Vector3 WithX(float newX)
    {
        return new Vector3(newX, 0, 0);
    }

    /// <summary>
    /// Creates a Vector3 with the specified y component and x and z components set to zero.
    /// </summary>
    public static Vector3 WithY(float newY)
    {
        return new Vector3(0, newY, 0);
    }

    /// <summary>
    /// Creates a Vector3 with the specified z component and x and y components set to zero.
    /// </summary>
    public static Vector3 WithZ(float newZ)
    {
        return new Vector3(0, 0, newZ);
    }
}
