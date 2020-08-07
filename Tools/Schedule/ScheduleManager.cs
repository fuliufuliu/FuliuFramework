using System;
using System.Collections.Generic;
using System.Threading;
using fuliu.pseudocode;
using UnityEngine;

namespace fuliu.Schedule
{
     /// <summary>
     /// 日程管理器
     /// </summary>
     public class ScheduleManager : SingleBhv<ScheduleManager>
     {
          private ScheduleData scheduleData;
          private Queue<Schedule> excuteQueue = new Queue<Schedule>();
          private Queue<Schedule> completeQueue = new Queue<Schedule>();
          private Queue<Schedule> repeatExcuteQueue = new Queue<Schedule>();
          private string saveKey = "ssefesfesfxxx";
          object obj = new object();
          public bool isNeedSaveOnQuit = true;

          public override void Init()
          {
               base.Init();
               scheduleData = Load();
          }

          #region 数据载入和保存

          private ScheduleData Load()
          {
               return Load<ScheduleData>();
          }


          private void OnApplicationQuit()
          {
               if (isNeedSaveOnQuit)
               {
                    Save(scheduleData, true);
               }
          }

          private void OnDestroy()
          {
               if (isNeedSaveOnQuit)
               {
                    Save(scheduleData, true);
               }
          }

          private void OnApplicationPause(bool pauseStatus)
          {
               if (pauseStatus)
               {
                    Debug.Log($"pauseStatus : {pauseStatus}, 暂停时保存");
                    // 暂停时保存
                    if (isNeedSaveOnQuit)
                    {
                         Save(scheduleData, true);
                    }
               }
               else
               {
                    Debug.Log($"pauseStatus : {pauseStatus}, 暂停回来？需要重新启动Schedule么？");
//                    ContinueAll();
               }
          }

          T Load<T>() where T : new()
          {
               var saveDataStr = PlayerPrefs.GetString(saveKey + typeof(T).Name, "wu");
               if (saveDataStr == "wu")
               {
                    Debug.LogWarning($"未加載到數據：{typeof(T).Name}");
                    return new T();
               }
               else
               {
                    Debug.Log($"{typeof(T).Name}  json：{saveDataStr}");
                    return JsonUtil.ToObject<T>(saveDataStr);
               }
          }

          void Save<T>(T obj, bool isSaveNow = false) where T : new()
          {
               Debug.Log($"Save  {typeof(T).Name}  json：{JsonUtil.ToJson(obj)}");
               PlayerPrefs.SetString(saveKey + obj.GetType().Name, JsonUtil.ToJson(obj));
               if (isSaveNow)
               {
                    PlayerPrefs.Save(); 
               }
          }

          #endregion

          public bool HasSchedule(string scheduleKey)
          {
               return scheduleData.schedules.ContainsKey(scheduleKey);
          }
          
          public Schedule GetSchedule(string scheduleKey)
          {
               if (scheduleData.schedules.ContainsKey(scheduleKey)) 
                    return scheduleData.schedules[scheduleKey];
               return null;
          }
     
          public Schedule AddSchedule(Schedule schedule)
          {
               if (schedule != null)
               {
                    scheduleData.schedules[schedule.key] = schedule;
                    CreateScheduleTimer(schedule);
               }

               return schedule;
          }
          
          public void RemoveSchedule(string scheduleKey, bool isExcuteScheduleComplete = false)
          {
               if (scheduleData.schedules.ContainsKey(scheduleKey))
               {
                    var schedule = scheduleData.schedules[scheduleKey];
                    scheduleData.schedules.Remove(scheduleKey);
                    if (schedule.timer != null)
                    {
                         schedule.timer.Dispose();
                         schedule.timer = null;
                    }
                    if (isExcuteScheduleComplete && !string.IsNullOrWhiteSpace(schedule.OnComplete))
                    {
                         completeQueue.Enqueue(schedule);
                    }
               }
          }
     
          private void CreateScheduleTimer(Schedule schedule)
          {
               schedule.timer = new Timer((obj) => ToExcuteScheduleEvent(schedule), schedule, 
                    schedule.delay, schedule.period);
          }
     
          private void ToExcuteScheduleEvent(Schedule schedule)
          {
               lock (obj)
               {
                    // 先执行后检查删除
                    excuteQueue.Enqueue(schedule);
                    schedule.excuteCount++;
                    schedule.lastExcuteTime = DateTime.Now;
                    if (schedule.maxExcuteCount == 1)
                    {
                         RemoveSchedule(schedule.key, true);
                    }
                    else if (schedule.maxExcuteCount > 1) //
                    {
                         RemoveSchedule(schedule.key, true);
                    }
               }
          }
     
          /// <summary>
          /// 接着执行所有的 schedule
          /// </summary>
          public void ContinueAll()
          {
               foreach (var keyValue in scheduleData.schedules)
               {
                    var schedule = keyValue.Value;
                    if (schedule.timer != null)
                    {
                         schedule.timer.Dispose();
                    }
     
                    RecomeInGameRefreshData(schedule);
                    CreateScheduleTimer(schedule);
               }
          }
     
          /// <summary>
          /// 重新进游戏，检查哪些事项未执行，未执行的事项现在执行一下
          /// </summary>
          /// <param name="schedule"></param>
          private void RecomeInGameRefreshData(Schedule schedule)
          {
               var excuteCount = schedule.excuteCount;
     
               var isFirstExcuted = excuteCount > 0;
               if (! isFirstExcuted)
               {
                    if (DateTime.Now > schedule.startTime + schedule.delay)
                    {
                         excuteQueue.Enqueue(schedule);
                         schedule.excuteCount++;
                         schedule.lastExcuteTime = schedule.startTime + schedule.delay;
                         if (schedule.maxExcuteCount == 1)
                         {
                              RemoveSchedule(schedule.key, true);
                         }
                    }
               }
     
               var maxExcuteCount = 0;
               if (schedule.maxExcuteCount < 0)
               {
                    maxExcuteCount = int.MaxValue;
               }
     
               // 到现在为止最多有多少次没执行
               var noExcuteCount = (DateTime.Now - schedule.lastExcuteTime).Ticks / schedule.period.Ticks;
               // 总共还需执行多少次
               var needExcuteCount = maxExcuteCount - schedule.excuteCount;
               // 现在可以立刻执行几次？
               var count = Mathf.CeilToInt(Mathf.Min(noExcuteCount, needExcuteCount));
               if (count > 0)
               {
                    schedule.repeatCount = count;
                    repeatExcuteQueue.Enqueue(schedule);
                    schedule.excuteCount += count;
                    // 是否执行完了？
                    if (count == needExcuteCount)
                    {
                         RemoveSchedule(schedule.key, true);
                    }
               }     
     
               while (repeatExcuteQueue.Count > 0)
               {
                    var _schedule = repeatExcuteQueue.Dequeue();
                    RepeatDoScheduleEvent(_schedule);
               }
          }

         
          private void Update()
          {
               lock (obj)
               {
                    for (int i = 0; i < 50; i++)
                    {
                         if (excuteQueue.Count == 0)
                         {
                              break;
                         }
     
                         var schedule = excuteQueue.Dequeue();
                         DoScheduleEvent(schedule);
                    }
                    for (int i = 0; i < 50; i++)
                    {
                         if (completeQueue.Count == 0)
                         {
                              break;
                         }
     
                         var schedule = completeQueue.Dequeue();
                         DoScheduleCompleteEvent(schedule);
                    }
               }
          }
          
     
          /// <summary>
          /// 在主线程执行日程的完成事件
          /// </summary>
          /// <param name="schedule"></param>
          /// <exception cref="NotImplementedException"></exception>
          private void DoScheduleCompleteEvent(Schedule schedule)
          {
               if (string.IsNullOrEmpty(schedule.OnComplete))
               {
                    Debug.Log($"{schedule.key} 的 日程 OnComplete 为空！");
                    return;
               }
               PseudocodeHelper.Run(schedule.OnComplete, null);
          }
     
          /// <summary>
          /// 在主线程调用日程事件
          /// </summary>
          /// <param name="schedule"></param>
          /// <exception cref="NotImplementedException"></exception>
          private void DoScheduleEvent(Schedule schedule)
          {
               if (string.IsNullOrEmpty(schedule.DoSomething))
               {
                    Debug.LogWarning($"{schedule.key} 的 日程 DoSomething 为空！");
                    return;
               }
               PseudocodeHelper.Run(schedule.DoSomething, null);
          }
               
          private void RepeatDoScheduleEvent(Schedule schedule)
          {
               if (string.IsNullOrEmpty(schedule.RepeatDoSomething))
               {
                    Debug.LogWarning($"{schedule.key} 的 日程 RepeatDoSomething 为空！");
                    return;
               }
               PseudocodeHelper.Run(string.Format(schedule.RepeatDoSomething, schedule.repeatCount));
               schedule.repeatCount = 0;
          }
     }
}
