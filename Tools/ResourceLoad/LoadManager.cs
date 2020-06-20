using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public static class LoadManager
{
    public static T Load<T>(string resourcesPath) where T : Object
    {
        return Resources.Load<T>(resourcesPath);
    }

    public static async Task<T> LoadAsync<T>(string resourcesPath) where T : Object
    {
        return await _LoadAsync<T>(resourcesPath);
    }
        
    public static IEnumerator<T> _LoadAsync<T>(string resourcesPath) where T : Object
    {
        yield return (T)Resources.LoadAsync<T>(resourcesPath).asset;
    }
    
    public static void LoadAsync<T>(string resourcesPath, Action<T> callback) where T : Object
    {
        CoroutineHelper.Instance.StartCoroutine(_LoadAsync(resourcesPath, callback));
    }

    private static IEnumerator _LoadAsync<T>(string resourcesPath, Action<T> callback) where T : Object
    {
        var resCor = Resources.LoadAsync<T>(resourcesPath);
        yield return resCor;
        callback?.Invoke((T)resCor.asset);
    }

}
