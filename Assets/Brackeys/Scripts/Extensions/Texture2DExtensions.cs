using System.Buffers;
using UnityEngine;

public static class Texture2DExtensions
{
    /// <summary>
    /// Returns the pixels from <paramref name="texture"/> as an array of floats in range 0...1.
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static float[] GreyscalePixels(this Texture2D texture)
    {
        var pixels = texture.GetPixelData<Color32>(0);
        float[] result = new float[pixels.Length];
        for (int index = 0; index < pixels.Length; index++)
        {
            result[index] = pixels[index].Greyscale();
        }
        return result;
    }

    /// <summary>
    /// Fills all pixels of a <see cref="Texture2D"> with a specified color.
    /// </summary>
    /// <param name="texture">The <see cref="Texture2D"> to fill.</param>
    /// <param name="color">The color to fill the texture with.</param>
    public static void SetPixelsToColor(this Texture2D texture, Color32 color)
    {
        SetPixelsToColor(texture, color, new RectInt(0, 0, texture.width, texture.height));
    }

    /// <summary>
    /// Fills a specific region of a <see cref="Texture2D"> with a specified color.
    /// </summary>
    /// <param name="texture">The <see cref="Texture2D"> to fill.</param>
    /// <param name="color">The color to fill the rectangle with.</param>
    /// <param name="rect">The rectangle defining the region to fill.</param>
    public static void SetPixelsToColor(this Texture2D texture, Color32 color, RectInt rect)
    {
        /// <summary>
        /// The width of the rectangle to fill.
        /// </summary>
        int width = rect.width;

        /// <summary>
        /// The height of the rectangle to fill.
        /// </summary>
        int height = rect.height;

        // Rent an array of colors from the shared ArrayPool.
        var colors = ArrayPool<Color32>.Shared.Rent(width * height);

        try
        {
            int index = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors[index] = color;
                    index++;
                }
            }

            // Set the pixels of the texture using the rented colors.
            texture.SetPixels32(rect.x, rect.y, width, height, colors);
        }
        finally
        {
            // Return the rented colors back to the ArrayPool.
            ArrayPool<Color32>.Shared.Return(colors);
        }
    }
}
