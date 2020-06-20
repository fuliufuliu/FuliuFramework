using UnityEngine;

/// <summary>
/// single struct
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleInstance<T> where T : new() {
    public static T m_instance;
    private static Object locked = new Object();

    public static T GetInstance() {
        if (m_instance == null) {
            lock (locked) {
                if (m_instance == null) {
                    m_instance = new T();
                }
            }
        }
        return m_instance;
    }

    public static T Instance {
        get { return GetInstance(); }
        set { m_instance = value; }
    }
}
