using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

[ExecuteInEditMode]
/// <summary>
/// Multithreading support
/// </summary>
public class Loom : MonoBehaviour {
    private static Loom _current;
    private int _count;
    /// <summary>
    /// Return the current instance
    /// </summary>
    /// <value>
    /// 
    /// </value>
    public static Loom Current
    {
        get
        {
            if (!_initialized) Initialize();
            return _current;
        }
    }

    static bool _initialized;
    static int _threadId = -1;

    public static void Initialize() {

        var go = !_initialized;

        if (go && _threadId != -1 && _threadId != Thread.CurrentThread.ManagedThreadId)
            return;

        if (go) {
            var g = new GameObject("_Loom");
            g.hideFlags = HideFlags.HideAndDontSave;
            _current = g.AddComponent<Loom>();
            if (Application.isPlaying)
            {
                GameObject.DontDestroyOnLoad(g);
                Component.DontDestroyOnLoad(_current);
            }
            _initialized = true;
            _threadId = Thread.CurrentThread.ManagedThreadId;
        }

    }

    void OnDestroy() {
        _initialized = false;
    }

    private List<Action> _actions = new List<Action>();
    public class DelayedQueueItem {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
    private List<Action> actionsTemp = new List<Action>();

    /// <summary>
	/// Queues an action on the main thread
	/// </summary>
	/// <param name='action'>
	/// The action to execute
	/// </param>
	public static void QueueOnMainThread(Action action) {
        QueueOnMainThread(action, 0f);
    }
    /// <summary>
    /// Queues an action on the main thread after a delay
    /// </summary>
    /// <param name='action'>
    /// The action to run
    /// </param>
    /// <param name='time'>
    /// The amount of time to delay
    /// </param>
    public static void QueueOnMainThread(Action action, float time) {
        if (time != 0) {
            lock (Current._delayed) {
                Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
            }
        }
        else {
            lock (Current._actions) {
                Current._actions.Add(action);
            }
        }
    }

    /// <summary>
    /// Runs an action on another thread
    /// </summary>
    /// <param name='action'>
    /// The action to execute on another thread
    /// </param>
    public static Thread RunAsync(Action action) {
        if (!_initialized) {
            Initialize();
        }
        var t = new Thread(RunAction);
        t.Priority = System.Threading.ThreadPriority.Normal;
        t.Start(action);
        return t;
    }

    public static Thread RunAsync(Action action, Action queueOnMainThreadAction)
    {
        if (!_initialized)
        {
            Initialize();
        }
        var t = new Thread(RunAction);
        t.Priority = System.Threading.ThreadPriority.Normal;
        t.Start(new Action(()=> {
            action();
            QueueOnMainThread(queueOnMainThreadAction);
        })
        );
        return t;
    }

    private static void RunAction(object action) {
        ((Action)action)();
    }


    // Update is called once per frame
    void Update() {
        actionsTemp.Clear();
        lock (_actions) {
            actionsTemp.AddRange(_actions);
            _actions.Clear();
            foreach (var a in actionsTemp) {
                a();
            }
        }
        lock (_delayed) {
            foreach (var delayed in _delayed.Where(d => d.time <= Time.time).ToList()) {
                _delayed.Remove(delayed);
                delayed.action();
            }
        }
    }
}