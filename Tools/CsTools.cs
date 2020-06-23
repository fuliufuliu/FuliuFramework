using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Random = UnityEngine.Random;

namespace fuliu
{
    public static class CsTools
    {
        public static T To<T>(this object obj)
        {
            if (obj is T obj1)
            {
                return obj1;
            }
            return (T) obj;
        }
        
        public static T As<T>(this object obj) where T:class
        {
            return obj as T;
        }
        
        
        /// <summary>
        /// 假设有一个字典，其value为列表，获取所有列表中元素数量之和
        /// </summary>
        public static int GetValueItemCount<T, T1>(this Dictionary<T, List<T1>> dic)
        {
            return dic?.Sum(pair => pair.Value?.Count ?? 0) ?? 0;
        }
       
        /// <summary>
        /// 获取一个正整数由哪几个 2的n次方数相加组成，把这几个数的次方数组成的列表返回
        /// </summary>
        /// <param name="v">正整数</param>
        /// <typeparam name="T">int, uint, long, ulong</typeparam>
        /// <returns>次方数list，如：1，2，3，4，5，。。。</returns>
        public static List<int> Get2PowList<T>(this T v) where T:struct
        {
            var res = new List<int>();
            var pow = 0;
            if (v is long l)
            {
                pow = 64;
                for (int i = 0; i < pow; i++)
                {
                    var w = l >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(i);
                }
            }
            else if (v is ulong ul)
            {
                pow = 63;
                for (int i = 0; i < pow; i++)
                {
                    var w = ul >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(i);
                }
            }            
            else if (v is int I)
            {
                pow = 31;
                for (int i = 0; i < pow; i++)
                {
                    var w = I >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(i);
                }
            }
            else if (v is uint uI)
            {
                pow = 32;
                for (int i = 0; i < pow; i++)
                {
                    var w = uI >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(i);
                }
            }
            return res;
        }
        
        /// <summary>
        /// 获取一个正整数由哪几个 2的n次方数相加组成，把这几个数的次方数组成的列表返回
        /// </summary>
        /// <param name="v">正整数</param>
        /// <typeparam name="T">int, uint, long, ulong</typeparam>
        /// <returns>次方计算结果的数list， 1，2，4，8，。。。</returns>
        public static List<int> Get2PowValueList<T>(this T v) where T:struct
        {
            var res = new List<int>();
            var pow = 0;
            if (v is long l)
            {
                pow = 64;
                for (int i = 0; i < pow; i++)
                {
                    var w = l >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(1<<i);
                }
            }
            else if (v is ulong ul)
            {
                pow = 63;
                for (int i = 0; i < pow; i++)
                {
                    var w = ul >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(1<<i);
                }
            }            
            else if (v is int I)
            {
                pow = 31;
                for (int i = 0; i < pow; i++)
                {
                    var w = I >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(1<<i);
                }
            }
            else if (v is uint uI)
            {
                pow = 32;
                for (int i = 0; i < pow; i++)
                {
                    var w = uI >> i;
                    var num = w % 2;
                    if(num == 1)res.Add(1<<i);
                }
            }
            return res;
        }
    }
}