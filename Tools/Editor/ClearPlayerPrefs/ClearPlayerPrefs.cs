using UnityEngine;
using UnityEditor;

namespace fuliu
{
    public static class ClearPlayerPrefs
    {
        [MenuItem("Tools/ClearPlayerPrefs")]
        public static void Clear()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("清除所有的 PlayerPrefs 数据！");
        }
    }
}