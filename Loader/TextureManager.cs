using UnityEngine;
using UnityEngine.UI;

public static class TextureManager
{
    public static void SetTexture(RawImage image, string resourcePath)
    {
        if (image)
        {
            image.enabled = false;
            LoadManager.LoadAsync<Texture2D>(resourcePath, (texture) =>
            {
                if (texture && image)
                {
                    image.enabled = true;
                    image.texture = texture; 
                }
            });
        }
    }
}
