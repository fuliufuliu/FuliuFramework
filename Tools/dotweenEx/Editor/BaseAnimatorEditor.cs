using UnityEditor;
using UnityEngine;

public abstract class BaseAnimatorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        BaseAnimator anim = target as BaseAnimator;
        serializedObject.Update();
        Gui(anim);
        GuiBack(anim);
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void Gui(BaseAnimator anim)
    {
        if (!anim.isEnable)
        {
            GUI.backgroundColor = Color.red;
            showProperty("isEnable");
            EditorGUILayout.LabelField("已禁用！要开启请点击下方的 IsEnable 选框", EditorStyles.textField);
            return;
        }
        else
        {
            GUI.backgroundColor = Color.blue;
            showProperty("isEnable");
        }
    }

    protected virtual void GuiBack(BaseAnimator anim)
    {
        if (!anim.isEnable)
        {
            return;
        }
        GUI.backgroundColor = Color.yellow;
        EditorGUILayout.LabelField("Common Attributes", EditorStyles.objectFieldThumb);
        showProperty("nextGameObjects");
        showProperty("disactiveDelay");
        showProperty("isDisactiveOnEnd");
        showProperty("customKey");
        GUI.backgroundColor = Color.magenta;
        //        showProperty("togetherGameObjects");
        showProperty("eventObjects");
    }

    protected void showProperty(string propertyName, bool isIncludeChidren = true)
    {
        var property = serializedObject.FindProperty(propertyName);
        if (property != null)
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), isIncludeChidren);
        else
            Debug.LogErrorFormat("在DoTweenAnimatior中未找到属性：{0}", propertyName);
    }
}

