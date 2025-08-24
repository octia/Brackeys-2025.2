using UnityEngine.UI;

public static class ImageExtensions
{
    /// <summary>
    /// Sets the image's alpha to 1 if unlocked, or 0.5f if locked. Leaves RGB values unchanged.
    /// </summary>
    public static void SetUnlocked(this Image image, bool isUnlocked)
    {
        var targetAlpha = isUnlocked ? 1f : 0.5f;
        image.color = image.color.WithA(targetAlpha);
    }
}
