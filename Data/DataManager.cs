using System.Collections.Generic;
using fuliu.Schedule;
using UnityEngine;

public interface IDataManager
{
    void Save<T>(T obj, bool isSaveNow = false) where T : new();
}

public abstract class DataManager<T> : SingleBhv<T>, IDataManager  where T : MonoBehaviour
{
    public bool isClearOnStart = false;
    
    protected string saveKey = "sefjjsajfejjjj";
    public SettingData settingData;
    protected bool isNeedSaveOnQuit = true;

    public static bool isShowMatchLog = true;
    
    public List<BaseData> datas = new List<BaseData>();


    public override void Initial()
    {
        base.Initial();
        
        if (Instance && Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void Load()
    {
        if(isClearOnStart)
            PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(gameObject);
        settingData = Load<SettingData>();
        OnLoad();
    }

    protected abstract void OnLoad();
    protected abstract void OnSaveAll();
    
    public void SaveAll()
    {
        Save(settingData);
        OnSaveAll();
        PlayerPrefs.Save();
    }
    
    protected T Load<T>() where T : new()
    {
        var saveDataStr = PlayerPrefs.GetString(saveKey + typeof(T).Name, "wu");
        if (saveDataStr == "wu")
        {
            Debug.LogWarning($"未加載到數據：{typeof(T).Name}");
            return new T();
        }
        else
        {
//            Debug.Log($"{typeof(T).Name}  json：{saveDataStr}");
            return JsonUtil.ToObject<T>(saveDataStr);
        }
    }

    public void Save<T>(T obj, bool isSaveNow = false) where T : new()
    {
//        Debug.Log($"Save  {typeof(T).Name}  json：{JsonUtil.ToJson(obj)}");
        if (obj != null)
        {
            PlayerPrefs.SetString(saveKey + obj.GetType().Name, JsonUtil.ToJson(obj));
            if (isSaveNow)
            {
                PlayerPrefs.Save(); 
            }
        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
        isNeedSaveOnQuit = false;
        ScheduleManager.Instance.isNeedSaveOnQuit = false;
        Application.Quit();
    }
    
    protected void OnApplicationQuit()
    {
        if (isNeedSaveOnQuit)
        {
            SaveAll();
        }
    }


    protected void OnDestroy()
    {
        if (isNeedSaveOnQuit)
        {
            SaveAll();
        }
    }

    protected void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // 暂停时保存
            if (isNeedSaveOnQuit)
            {
                SaveAll();
            }
        }
    }
}

namespace __Example__
{
    public class MyDataManager : DataManager<MyDataManager>
    {
        public PlayerData playerData;
        public GlobalData globalData;

        protected override void OnLoad()
        {
            playerData = Load<PlayerData>();
            globalData = Load<GlobalData>();
        }

        protected override void OnSaveAll()
        {
            playerData.Save(this);
            globalData.Save(this);
        }
    }

    public class PlayerData : BaseData
    {
        public string filePath;
    }
    
    public class GlobalData : BaseData
    {
        
    }
}
