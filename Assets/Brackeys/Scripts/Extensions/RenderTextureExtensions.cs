using UnityEngine;

public static class RenderTextureExtensions
{
    /// <summary>
    /// Converts the <see cref="RenderTexture"/> to a <see cref="Texture2D"/> using RGBA32 format.
    /// </summary>
    public static Texture2D ToTexture2D(this RenderTexture renderTexture)
    {
        var texture = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            TextureFormat.RGBA32,
            false
        );
        var oldRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        RenderTexture.active = oldRenderTexture;
        return texture;
    }
}
