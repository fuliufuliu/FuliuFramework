using System;
using System.Threading;
using fuliu.Schedule;
using fuliu.serialize;

namespace fuliu.Schedule
{
    [Serializable]
    public class Schedule
    {
        /// <summary>
        /// 作为查询用的 key
        /// </summary>
        public string key;

        /// <summary>
        /// 第一次执行延迟时间，<= 0 表示无延迟立刻执行（事实上可能还是有一点点延迟，它要等待执行检查程序检查到可执行才会执行）
        /// </summary>
        public TimeSpan delay;

        /// <summary>
        /// 循环周期
        /// </summary>
        public TimeSpan period;

        /// <summary>
        /// 被执行次数
        /// </summary>
        public int excuteCount;

        /// <summary>
        /// 最大可执行次数, -1 无限循环
        /// </summary>
        public int maxExcuteCount;

        /// <summary>
        /// 最近一次执行的时间
        /// </summary>
        public DateTime lastExcuteTime;

        /// <summary>
        /// 做什么事情？
        /// </summary>
        public string DoSomething;

        /// <summary>
        /// 重复做某事，主要用于立刻执行多次 DoSomething的情况。需要在字符串中留出填入重复次数的参数位置
        /// </summary>
        public string RepeatDoSomething;

        /// <summary>
        /// 重复做某事的次数，执行完后最好手动清掉
        /// </summary>
        public int repeatCount;

        /// <summary>
        /// 最后一次执行完做什么？
        /// </summary>
        public string OnComplete;

        [NonSerialized] public Timer timer;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime startTime;


        public Schedule(string key, TimeSpan delay, string DoSomething, string RepeatDoSomething,string OnComplete = null)
        {
            this.key = key;
            this.delay = delay;
            lastExcuteTime = startTime = DateTime.Now;
            this.DoSomething = DoSomething;
            this.RepeatDoSomething = RepeatDoSomething;
        }

//    public Schedule(){}

        public Schedule SetLoop(TimeSpan period, int maxExcuteCount)
        {
            this.period = period;
            this.maxExcuteCount = maxExcuteCount;
            return this;
        }

    }

    [Serializable]
    public class ScheduleData
    {
        public StringScheduleDic schedules = new StringScheduleDic();
    }
}

public class StringScheduleDic : SerDictionary<string, Schedule>{}