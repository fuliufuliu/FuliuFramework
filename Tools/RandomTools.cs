using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class RandomTools
{
    /// <summary>
    /// weightMap 中每项的权重必须为正，不然是随机不到的。
    /// </summary>
    /// <param name="weightMap"></param>
    /// <param name="seed"></param>
    public static T WeightSelect<T>(Dictionary<T, float> weightMap, int seed = 0)
    {
        if (weightMap == null)
        {
            throw new ArgumentNullException(" weightMap ");
        }

        var allWeight = 0f;
        foreach (var pair in weightMap)
        {
            allWeight += pair.Value;
        }

        if (seed != 0)
        {
            Random.InitState(seed);
        }
        var randValue = Random.Range(0, allWeight);

        var tList = weightMap.Keys.ToList();
        var t = 0f;
        for (int i = 0; i < tList.Count; i++)
        {
            t += weightMap[tList[i]];
            if (t > randValue)
            {
                return tList[i];
            }
        }

        return tList[0];
    }

    public static T Choice<T>(IList<T> list, IList<T> exceptList = null)
    {
        if (exceptList == null)
        {
            return list[Random.Range(0, list.Count)];
        }
        else
        {
            int count = 0;
            rerandom:
            var rand = list[Random.Range(0, list.Count)];
            if (exceptList.Contains(rand))
            {
                count++;
                if (count > 100)
                {
                    throw new Exception("Choice Loop 次数太多！");
                }
                goto rerandom;
            }

            return rand;
        }
    }
}
