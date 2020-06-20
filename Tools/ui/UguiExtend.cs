using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class UguiExtend
{
    public static Toggle[] getToggleGroupActiveToggles(this ToggleGroup toggleGroup)
    {
        return toggleGroup.ActiveToggles().ToArray();
    }

    public static string getSpriteName(this Image image)
    {
        if (image != null)
        {
            if (image.overrideSprite)
            {
                return image.overrideSprite.name;
            }
            if (image.sprite)
            {
                return image.sprite.name;
            }
        }
        return null;
    }

    public static void setSpriteName(this Image image, string spriteName, UguiAtlas uguiAtlas)
    {
        if (image != null && uguiAtlas != null)
        {
            image.overrideSprite = uguiAtlas.getSprite(spriteName);
        }
    }

//    public static Vector3 WorldToUI(Camera camera, Vector3 pos)
//    {
//        CanvasScaler scaler = GameObject.Find("UIRoot").GetComponent<CanvasScaler>();
//
//        float resolutionX = scaler.referenceResolution.x;
//        float resolutionY = scaler.referenceResolution.y;
//
//        Vector3 viewportPos = camera.WorldToViewportPoint(pos);
//
//        Vector3 uiPos = new Vector3(viewportPos.x * resolutionX - resolutionX * 0.5f,
//            viewportPos.y * resolutionY - resolutionY * 0.5f, 0);
//
//        return uiPos;
//    }
    public static Vector2 WorldToUIPoint(this Canvas canvas, Camera gameCamera, Transform worldGo)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            gameCamera.WorldToScreenPoint(worldGo.transform.position), canvas.worldCamera, out pos);
        return pos;
    }
}
