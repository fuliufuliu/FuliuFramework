using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace fuliu
{
    public class SceneManager : SingleBhv<SceneManager>
    {
        private RectTransform uiRoot;
        private string uiPrefabPathRoot;
        Dictionary<string, GameObject> uiDic = new Dictionary<string, GameObject>();
        Dictionary<GameObject, string> goDic = new Dictionary<GameObject, string>();

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
        
        /// <summary>
        /// 打开一个UI界面
        /// </summary>
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

            Record(uiName, uiGo);

            return uiGo;
        }

        private void Record(string uiName, GameObject uiGo)
        {
            uiDic[uiName] = uiGo;
            goDic[uiGo] = uiName;
        }

        /// <summary>
        /// 打开一个UI界面，并关闭其他所有界面
        /// </summary>
        public GameObject OpenOnlyUI(string uiName, params object[] parameters)
        {
            var go = OpenUI(uiName, parameters);
            CloseAllUI(new HashSet<string>() {uiName});
            return go;
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
                var go = uiDic[uiName];
                goDic.Remove(go);
                uiDic.Remove(uiName);
                Destroy(go);
                return true;
            }

            return false;
        }
        
        public bool CloseUI(GameObject uiObj)
        {
            if (!uiRoot)
            {
                Debug.LogError($"SceneManager 需要初始化属性：uiRoot！");
                return false;
            }

            if (goDic.ContainsKey(uiObj)) 
                return CloseUI(goDic[uiObj]);
            
            return false;
        }
        
        public bool CloseAllUI(HashSet<string> butThese)
        {
            if (!uiRoot)
            {
                Debug.LogError($"SceneManager 需要初始化属性：uiRoot！");
                return false;
            }

            var list = new List<string>();
            foreach (var pair in uiDic)
            {
                if (butThese.Contains(pair.Key))
                {
                    continue;
                }
                list.Add(pair.Key);
            }

            foreach (var uiName in list)
            {
                CloseUI(uiName);
            }

            return false;
        }

        public bool HasUI(string uiName)
        {
            return uiDic.ContainsKey(uiName);
        }
    }
}