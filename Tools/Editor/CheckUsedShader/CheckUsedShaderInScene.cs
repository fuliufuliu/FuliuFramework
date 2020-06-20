using UnityEditor;
using UnityEngine;

namespace fuliu
{
    public static class CheckUsedShader
    {
        [MenuItem("Tools/Check Used Shader   In Current Scene")]
        public static void Check()
        {
            var renderers = GameObject.FindObjectsOfType<Renderer>();
            if (renderers != null)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    var sharedMaterials = renderers[i].sharedMaterials;
                    if (sharedMaterials != null)
                    {
                        for (int j = 0; j < sharedMaterials.Length; j++)
                        {
                            if (sharedMaterials[j])
                            {
                                var shader = sharedMaterials[j].shader;
                                if (shader != null)
                                    if (shader.name.Contains("Standard"))
                                    {
                                         Debug.LogFormat("--- 检查 {0}, shader = <color=#ff0000>  {1}  </color>",
                                                                                renderers[i].gameObject.name,
                                                                                shader.name);
                                    }
                                    else
                                    {
                                        Debug.LogFormat("--- 检查 {0}, shader = {1}",
                                            renderers[i].gameObject.name,
                                            shader.name);
                                    }
                                else
                                {
                                    Debug.LogFormat("######### 检查 {0}, shader = None",
                                        renderers[i].gameObject.name );
                                }
                            }
                            else
                            {
                                Debug.LogFormat("$$$$$$$$$$$$ 检查 {0}, material = None",
                                    renderers[i].gameObject.name );
                            }
                        }
                    }
                }
                Debug.Log("检查完毕！"); 
            }
            else
            {
                Debug.Log("在当前场景中没有找到任何Renderer！检查完毕！"); 
            }
        }
    }
}
