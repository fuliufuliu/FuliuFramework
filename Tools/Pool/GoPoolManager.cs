using System.Collections.Generic;
using UnityEngine;

public class GoPoolManager : SingleBhv<GoPoolManager>
{
    Dictionary<string, GameObject> templetesDic = new Dictionary<string, GameObject>();
    Dictionary<string, Stack<GameObject>> poolsDic = new Dictionary<string, Stack<GameObject>>();

    public override void Initial()
    {
        base.Initial();
        if (Instance && Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        // DontDestroyOnLoad(gameObject);
    }

    public override void Init()
    {
        base.Init();
        
        gameObject.SetActive(false);
    }

    public void CreatePool(string poolKey, GameObject goTemplete, bool isForce = true)
    {
        if (! isForce && poolsDic.ContainsKey(poolKey) && templetesDic[poolKey])
        {
            Debug.LogWarning($"池中已经存在 Key：{poolKey} ");
            return;
        }

        if (isForce)
        {
            if (poolsDic.ContainsKey(poolKey) && templetesDic[poolKey])
            {
                foreach (var go in poolsDic[poolKey])
                {
                    Destroy(go);
                }
            }
        }

        if (goTemplete != null)
        {
            templetesDic[poolKey] = goTemplete;
            poolsDic[poolKey] = new Stack<GameObject>();
        }
    }

    public bool HasPool(string poolKey)
    {
        return poolsDic.ContainsKey(poolKey);
    }
    
    public GameObject Pop(string poolKey)
    {
        if (! templetesDic.ContainsKey(poolKey))
        {
            Debug.LogError($"无 key：{poolKey} 的池！");
            return null;
        }
        var stack = poolsDic[poolKey];
        GameObject go;
        redo_pop:
        if (stack.Count > 0)
        {
            go = stack.Pop();
            if (! go)
            {
                goto redo_pop;
            }
            go.transform.SetParent(null);
        }
        else
        {
            go = Instantiate(templetesDic[poolKey]);
        }
        return go;
    }

    public void Push(string poolKey, GameObject go)
    {
        if (go)
        {
            if (poolsDic.ContainsKey(poolKey))
            {
                go.transform.SetParent(transform);
                poolsDic[poolKey].Push(go);
                go.SetActive(false);
            }
            else
            {
                Debug.LogError($"无 key：{poolKey} 的池！");
            }
        }
    }

    /// <param name="transPosRotScale">可根据此Transform的位置旋转比例来设置新gameobjct的transoform属性</param>
    public GameObject Pop(string poolKey, Transform parent, Transform transPosRotScale = null)
    {
        var newGo = Pop(poolKey);
        if (transPosRotScale == null)
        {
            transPosRotScale = templetesDic[poolKey].transform;
        }
        SetNewGoProps(newGo, parent, transPosRotScale).SetActive(true);
        return newGo;
    }

    public GameObject SetNewGoProps(GameObject newGo, Transform parent, Transform transPosRotScale)
    {
        newGo.transform.SetParent(parent);
        newGo.transform.position = transPosRotScale.position;
        newGo.transform.rotation = transPosRotScale.rotation;
        newGo.transform.localScale = transPosRotScale.localScale;
        return newGo;
    }
    
    public GameObject SetNewGoProps(GameObject newGo, Transform parent, Vector3 pos, bool isLocal = false)
    {
        newGo.transform.SetParent(parent);
        newGo.transform.Reset();
        if (isLocal)
        {
            newGo.transform.localPosition = pos;
        }
        else
        {
            newGo.transform.position = pos;
        }
        return newGo;
    }
}
