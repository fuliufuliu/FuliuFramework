using System;
using UnityEngine;

public abstract class SingleBhv<T> : MonoBehaviour where T : MonoBehaviour {
    private static T m_instance;
    private static object locked;
    /// <summary>
    /// 比Init方法之前检测
    /// </summary>
    public bool isInitialled;

    protected bool isInited;

    public static void SetInstance(T instance) {
        m_instance = instance;
    }

    public static T Instance {
        get {
            try {
                if (m_instance == null) {
                    m_instance = UnityEngine.Object.FindObjectOfType<T>();
                    InvokeInitial();
                }
            }
            catch (Exception ex) {
                Debug.LogError("FindObjectOfType 引起异常!"+ex);
            }
            if (m_instance == null) {
                m_instance = new GameObject("_" + typeof(T)).AddComponent<T>();
                InvokeInitial();
            }
            return m_instance;
        }
        set {
            m_instance = value;
        }
    }

    private static void InvokeInitial() {
        if (m_instance != null) {
            var instance = m_instance as SingleBhv<T>;
            if (!instance.isInitialled) {
                instance.Initial();
                instance.isInitialled = true;
            }
        }
    }

    /// <summary>
    /// 在Init之前调用，并且第一次使用Instance时直接调用，犹如构造函数
    /// </summary>
    public virtual void Initial() { }
    /// <summary>
    /// 不同于Initial方法，此方法可以等待Instance各属性初始化完成后再调用
    /// </summary>
    public virtual void Init() {
        m_instance = this as T;
        isInited = true;
        InvokeInitial();
    }



    /// <summary>
    /// 释放掉
    /// </summary>
    public static void Destroy() {
        if (m_instance) {
            GameObject.Destroy(m_instance.gameObject);
        }
    }
}