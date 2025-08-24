using UnityEngine;

public static class RectTransformExtensions
{
    /// <summary>
    /// Resets the <see cref="RectTransform"/> to fill its parent entirely,
    /// setting anchors to stretch, pivot to center, and zeroing size and position offsets.
    /// </summary>
    /// <param name="rt">The <see cref="RectTransform"/> to reset.</param>
    public static void StretchToParentFullRect(this RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// Calculates the total bounding size of all child elements inside this RectTransform.
    /// </summary>
    /// <param name="rect">The RectTransform whose content is measured.</param>
    /// <returns>Size of the combined bounds of all children.</returns>
    public static Vector2 GetContentBoundsSize(this RectTransform rect)
    {
        var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rect);
        return bounds.size;
    }
}
