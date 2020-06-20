#define _TimeTools_Debug
using System;

public static class TimeTools {
    
    public enum Period
    {
        /// <summary>
        /// 年
        /// </summary>
        Year,
        /// <summary>
        /// 季度
        /// </summary>
//        Quarter,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 日
        /// </summary>
        Day,
        Hour,
        Minute,
        Second,
    }

#if TimeTools_Debug
    static TimeTools()
    {
        Debug.Log("------------------------------------ 下一年的每个月第一天 "+Period.Year);
        for (int i = 0; i < 12; i++)
        {
            Debug.Log(GetNextTime(i+1, Period.Year));
        }
        Debug.Log("   ");
        
        Debug.Log("------------------------------------ 下一月的每个天开始"+Period.Month);
        for (int i = 0; i < 31; i++)
        {
            Debug.Log(GetNextTime(i+1, Period.Month));
        }
        Debug.Log("   ");
        
                
        Debug.Log("------------------------------------ 明天的24小时开始"+Period.Day);
        for (int i = 0; i < 24; i++)
        {
            Debug.Log(GetNextTime(i, Period.Day));
        }
        Debug.Log("   ");
        
                
        Debug.Log("------------------------------------"+Period.Hour);
        for (int i = 0; i < 60; i++)
        {
            Debug.Log(GetNextTime(i, Period.Hour));
        }
        Debug.Log("   ");
        
                
        Debug.Log("------------------------------------"+Period.Minute);
        for (int i = 0; i < 60; i++)
        {
            Debug.Log(GetNextTime(i, Period.Minute));
        }
        Debug.Log("   ");
        
                
        Debug.Log("------------------------------------"+Period.Second);
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(GetNextTime(i*100, Period.Second));
        }
        Debug.Log("   ");
    }
#endif
    
    /// <summary>
    /// 获取从现在开始算起，到下一个指定周期的时刻点用DateTime怎么表示？
    /// </summary>
    /// <returns></returns>
    public static DateTime GetNextTime(int value, Period period)
    {
        var now = DateTime.Now;
        switch (period)
        {
            case Period.Year:
                return now.AddYears(1).AddDays(1 - now.Day).AddMonths(-now.Month + value).AddHours(-now.Hour + 1).AddMinutes(-now.Minute)
                    .AddSeconds(-now.Second);
            case Period.Month:
                return new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1).AddDays(value - 1);
            case Period.Day:
                return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1).AddHours(value);
            case Period.Hour:
                return new DateTime(now.Year, now.Month, now.Day , now.Hour, 0, 0).AddHours(1).AddMinutes(value);
            case Period.Minute:
                return new DateTime(now.Year, now.Month, now.Day , now.Hour, now.Minute, 0).AddMinutes(1).AddSeconds(value);
            case Period.Second:
                return new DateTime(now.Year, now.Month, now.Day , now.Hour, now.Minute, now.Second, 0).AddSeconds(1).AddMilliseconds(value);
            default:
                throw new ArgumentOutOfRangeException(nameof(period), period, null);
        }
    }
    
    /// <summary>
    /// 获取从现在开始算起，到下一个指定一天中的时刻点用DateTime怎么表示？
    /// </summary>
    /// <param name="hour"> 0~23 表示一天中的几点</param>
    /// <param name="minute">0~59 表示一天中某点的几分</param>
    /// <param name="second">0~59 表示一天中某分钟的几秒</param>
    /// <returns></returns>
    public static DateTime GetNextTimeOfDay(int hour, int minute, int second)
    {
        var now = DateTime.Now;
        var nextTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
        if (nextTime < now)
        {
            return nextTime + TimeSpan.FromDays(1);
        }
        return nextTime;
    }
    
    /// <summary>
    /// 获取从现在开始算起，到下一个指定一天中的时刻点需要等待多久？
    /// </summary>
    /// <param name="hour"> 0~23 表示一天中的几点</param>
    /// <param name="minute">0~59 表示一天中某点的几分</param>
    /// <param name="second">0~59 表示一天中某分钟的几秒</param>
    /// <returns></returns>
    public static TimeSpan GetNextTimeNeedSpanOfDay(int hour, int minute, int second)
    {
        return  GetNextTimeOfDay(hour, minute, second) - DateTime.Now;
    }
}
