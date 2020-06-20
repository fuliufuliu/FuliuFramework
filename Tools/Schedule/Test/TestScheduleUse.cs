using System;
using fuliu.pseudocode;
using fuliu.Schedule;
using UnityEngine;

public class TestScheduleUse : SingleBhv<TestScheduleUse>
{
    public int test1CallCount = 0;
    public int test2CallCount = 0;
    private Schedule _Test1Schedule;
    private Schedule _Test2Schedule;

    // Start is called before the first frame update
    void Start()
    {
        PseudocodeHelper.AddPseudocodeFuncs(typeof(TestScheduleUse), MonoBehaviorFuncList.funcNames);

        ScheduleManager.Instance.Init();
        ScheduleManager.Instance.ContinueAll();
        if (! ScheduleManager.Instance.HasSchedule("Test1"))
        {
            Debug.Log($"添加日程：Test1");
            _Test1Schedule = ScheduleManager.Instance.AddSchedule(
                new Schedule("Test1", TimeSpan.FromSeconds(1),
                    "Print(\"test1CallCount : \",Test1Call())",
                    "Print(\"test1CallCount : \",RepeatTest1Call({0}))",
                    "").SetLoop(TimeSpan.FromSeconds(1), -1));
        }
        else
        {
            _Test1Schedule = ScheduleManager.Instance.GetSchedule("Test1");
        }
 
        if (! ScheduleManager.Instance.HasSchedule("Test2"))
        {
            Debug.Log($"添加日程：Test2");
            _Test2Schedule = ScheduleManager.Instance.AddSchedule(
                new Schedule("Test2", TimeSpan.FromSeconds(1.5f),
                    "Print(Test2Call())",
                    "").SetLoop(TimeSpan.FromSeconds(1.5f), -1));
        }
        else
        {
            _Test2Schedule = ScheduleManager.Instance.GetSchedule("Test2");
        }
    }

    public static void RepeatTest1Call(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters)
    {
        var _count = Mathf.CeilToInt( (float) parameters[0]);
        Debug.Log($"_count: {_count}");
        Instance.test1CallCount = _count;
        callback?.Invoke(Instance.test1CallCount);
    }
    
    
    public static void Test1Call(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters)
    {
        Instance.test1CallCount ++;
        callback?.Invoke(Instance.test1CallCount);
    }
    
    public static void Test2Call(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters)
    {
        Instance.test2CallCount ++;
        callback?.Invoke(Instance.test2CallCount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ScheduleManager.Instance.RemoveSchedule(_Test1Schedule.key);
            ScheduleManager.Instance.RemoveSchedule(_Test2Schedule.key);
            PlayerPrefs.DeleteAll();
            Debug.Log("删除永久化数据！");
        }
    }
}
