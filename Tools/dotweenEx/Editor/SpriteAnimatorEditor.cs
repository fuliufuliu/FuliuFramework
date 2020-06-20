using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteAnimator))]
public class SpriteAnimatorEditor : BaseAnimatorEditor
{
    protected override void Gui(BaseAnimator _anim)
    {
        SpriteAnimator anim = _anim as SpriteAnimator;
        base.Gui(anim);
        if (!anim.isEnable)
        {
            GUI.backgroundColor = Color.green;
            return;
        }
        else
        {
            GUI.backgroundColor = Color.blue;
        }
        EditorGUILayout.LabelField("Attributes", EditorStyles.objectFieldThumb);

        GUI.backgroundColor = Color.green;
      
        showProperty("playAwake");
        showProperty("findAtlas");
        if (anim.findAtlas)
        {
            EditorGUI.indentLevel++;
            showProperty("uguiAtlas");
            showProperty("prefix");
            EditorGUI.indentLevel--;
        }

        if (anim.findAtlas)
        {
            if (Application.isPlaying)
            {
                showProperty("frames");
            }
        }
        else
        {
            showProperty("frames");
        }
        showProperty("speed");
        showProperty("delay");
        showProperty("isShowBeforeDelay");
        showProperty("loopCount");
        showProperty("maxCount");
    }
}
