using System.Collections.Generic;
using UnityEngine;

namespace fuliu
{
    public class SceneManager : SingleBhv<SceneManager>
    {
        private RectTransform uiRoot;
        private string uiPrefabPathRoot;
        Dictionary<string, GameObject> uiDic = new Dictionary<string, GameObject>();

        /// <summary>
        /// 打开Unity场景， 无需初始化即可使用
        /// </summary>
        public static void Open(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
        /// <summary>
        /// 异步打开Unity场景， 无需初始化即可使用
        /// </summary>
        public static void OpenAsync(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        }
        
        /// <summary>
        /// 关闭Unity场景， 无需初始化即可使用
        /// </summary>
        public static void Close(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }

        public void InitParams(RectTransform uiRoot, string uiPrefabPathRoot)
        {
            this.uiRoot = uiRoot;
            uiPrefabPathRoot = uiPrefabPathRoot.Replace("\\\\", "/").Replace("\\", "/");
            if (! uiPrefabPathRoot.EndsWith("/"))
            {
                uiPrefabPathRoot += "/";
            }
            this.uiPrefabPathRoot = uiPrefabPathRoot;
        }

        public GameObject OpenUI(string uiName, params object[] parameters)
        {
            if (!uiRoot)
            {
                Debug.LogError($"SceneManager 需要初始化属性：uiRoot！");
                return null;
            }

            var uiGo = LoadManager.Load<GameObject>(GetUIPathName(uiName));
            uiGo = Instantiate(uiGo, uiRoot, true);
            uiGo.transform.Reset();
            var rectTrans = uiGo.transform as RectTransform;
            rectTrans.offsetMax = Vector2.zero;
            rectTrans.offsetMin = Vector2.zero;
            uiGo.SetActive(true);
            var ui = uiGo.GetComponent<UI>();
            if (ui)
            {
                ui.InitParams(parameters);
            }

            uiDic[uiName] = uiGo;
            return uiGo;
        }
        
        /// <summary>
        /// 可指定新加载的UI的层级
        /// </summary>
        /// <param name="sortingOrder">层级， 越大层级越高</param>
        public GameObject OpenUiOrder(string uiName, int sortingOrder, params object[] parameters)
        {
            var uiGo = OpenUI(uiName, parameters);
            var canvas = uiGo.transform.GetSafeComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
            return uiGo;
        }

        private string GetUIPathName(string uiName)
        {
            return uiPrefabPathRoot + uiName;
        }

        public bool CloseUI(string uiName)
        {
            if (!uiRoot)
            {
                Debug.LogError($"SceneManager 需要初始化属性：uiRoot！");
                return false;
            }

            if (uiDic.ContainsKey(uiName))
            {
                Destroy(uiDic[uiName]);
                uiDic.Remove(uiName);
                return true;
            }

            return false;
        }
    }
}