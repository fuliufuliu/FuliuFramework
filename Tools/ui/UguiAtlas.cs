using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UguiAtlas")]
public class UguiAtlas : ScriptableObject
{
    public Texture mainTexture;
    public Sprite[] sprites;
    public Dictionary<string, Sprite> spriteDic;
    private bool inited;

    public UguiAtlas()
    {
        init();
    }

    public void init()
    {
        inited = true;
        if (sprites != null)
        {
            spriteDic = new Dictionary<string, Sprite>(sprites.Length);
            for (int i = 0; i < sprites.Length; i++)
            {
                spriteDic[sprites[i].name] = sprites[i];
            }
        }
    }

    /// <summary>
    /// 获取sprite
    /// </summary>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite getSprite(string spriteName)
    {
        if (spriteDic == null && sprites != null && sprites.Length > 0)
        {
            init();
        }

        Sprite spr;

        if (!string.IsNullOrEmpty(spriteName) && spriteDic.TryGetValue(spriteName, out spr))
        {
            return spr;
        }
        return null;
    }
}

