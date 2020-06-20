using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace fuliu.pseudocode
{
    public static class DefaultFuncs
    {
        public static float GetFloat(this object Value)
        {
            if (Value is string str)
            {
                if (float.TryParse(str, out var v))
                {
                    return v;
                }
                if (int.TryParse(str, out var i))
                {
                    return i;
                }
            }
            if (Value is int value) return value;
            if (Value is float f) return f;
            return 0f;
        }
        
        public static int GetInt(this object Value)
        {
            if (Value is string str)
            {
                if (int.TryParse(str, out var i))
                {
                    return i;
                }               
                if (float.TryParse(str, out var v))
                {
                    return Mathf.RoundToInt(v);
                }
            }
            if (Value is int value) return value;
            if (Value is float f) return Mathf.RoundToInt(f);
            return 0;
        }        
        
        public static bool GetBool(this object Value)
        {
            if (Value is string str)
            {
                if (bool.TryParse(str.ToLower(), out var i))
                {
                    return i;
                }               
            }
            if (Value is bool value) return value;
            if (Value is int valueInt) return valueInt != 0;
            // float.Epsilon是大于零的最小浮点数(大约为1.401298E-45)
            if (Value is float valueF) return Math.Abs(valueF) > float.Epsilon;
            return false;
        }
        
        public static string GetStr(this object Value)
        {
            if (Value != null)
            {
                return Value.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取列表的指定元素， index超出范围不会报错，只会返回默认值
        /// </summary>
        public static T GetItem<T>(this IList<T> parameters, int index, T defaultValue = default)
        {
            if (parameters.Count > index)
            {
                return parameters[index];
            }
            return defaultValue;
        }
        
        public static void IsGreaterOrEqualsTo(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a >= b);
        }
        
        public static void IsGreaterTo(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a > b);
        }
        
        public static void IsLessOrEqualsTo(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a <= b);
        }
        
        public static void IsLessTo(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a < b);
        }
        
        public static void IsEqualsTo(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(Math.Abs(a - b) < 0.0000001f);
        }

        public static void SetVar(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            string varName = (string)parameters[0];
            object varValue = parameters[1];

            Pseudocode.vars[varName] = varValue;
            callback?.Invoke(varValue);
        }
        
        public static void GetVar(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            string varName = (string)parameters[0];

            if (Pseudocode.vars.ContainsKey(varName)) 
                callback?.Invoke(Pseudocode.vars[varName]);
            else
            {
                Debug.LogError($"不存在变量：{varName}");
            }
        }
        
        public static void RemoveVar(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            string varName = (string)parameters[0];

            var res = Pseudocode.vars[varName];
            Pseudocode.vars.Remove(varName);
            callback?.Invoke(res);
        }
        
        public static void If(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            bool isTrue = (bool)parameters[0];
            PseudocodeAction TrueDoSth = (PseudocodeAction)parameters[1];
            PseudocodeAction FalseDoSth = (PseudocodeAction)parameters[2];

            if (isTrue)
            {
                if (TrueDoSth != null)
                    Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(TrueDoSth.funcs),
                        () => { callback?.Invoke(null); }, commonParameters);
                else
                {
                    callback?.Invoke(null); 
                }
            }
            else
            {
                if (FalseDoSth != null)
                    Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(FalseDoSth.funcs),
                        () => { callback?.Invoke(null); }, commonParameters);
                else
                {
                    callback?.Invoke(null); 
                }
            }
        }
        
        public static void IF(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            if (parameters[0] is bool)
            {
                throw new Exception("定义IF方法的条件参数，需要使用方法: NewFunc(表达式)");
            }
            PseudocodeFunc isTruePseudocodeFunc = (PseudocodeFunc) parameters[0];
            PseudocodeAction TrueDoSth = (PseudocodeAction)parameters[1];
            PseudocodeAction FalseDoSth = (PseudocodeAction)parameters[2];

            Pseudocode.Run(isTruePseudocodeFunc, () =>
            {
                if ((bool) isTruePseudocodeFunc.result)
                {

                    if (TrueDoSth != null)
                        Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(TrueDoSth.funcs),
                            () => { callback?.Invoke(null); }, commonParameters);
                    else
                    {
                        callback?.Invoke(null);
                    }
                }
                else
                {
                    if (FalseDoSth != null)
                        Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(FalseDoSth.funcs),
                            () => { callback?.Invoke(null); }, commonParameters);
                    else
                    {
                        callback?.Invoke(null);
                    }
                }
            });
        }

        public static void While(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            if (parameters[0] is bool)
            {
                throw new Exception("定义While方法的条件参数，需要使用方法: NewFunc(表达式)");
            }
            PseudocodeFunc isTruePseudocodeFunc = (PseudocodeFunc) parameters[0];
            PseudocodeAction LoopDoSth = (PseudocodeAction) parameters[1];

            Pseudocode.Run(isTruePseudocodeFunc, () =>
            {
                if ((bool) isTruePseudocodeFunc.result)
                {
                    if (LoopDoSth != null)
                    {
                        Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(LoopDoSth.funcs),
                            () => { While(Pseudocode, funcName, callback, parameters, commonParameters); },
                            commonParameters);
                    }
                    else
                    {
                        callback?.Invoke(null);
                    }
                }
                else
                {
                    callback?.Invoke(null);
                }
            });
        }
        
        /// <summary>
        /// 指定循环次数连续做几次某事 
        /// </summary>
        public static void For(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            int loopCount = parameters[0].GetInt();
            PseudocodeAction LoopDoSth = (PseudocodeAction) parameters[1];
            string loopId = parameters.GetItem(2).GetStr();

            if (loopCount > 0)
            {
                loopCount--;
                parameters[0] = loopCount;

                if (loopId != null)
                {
                    Pseudocode.vars[loopId] = loopCount;
                }
                
                if (LoopDoSth != null)
                {
                    Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(LoopDoSth.funcs),
                        () => { For(Pseudocode, funcName, callback, parameters, commonParameters); }, commonParameters);
                }
                else
                {
                    callback?.Invoke(null);
                }
            }
            else
            {
                callback?.Invoke(null);
            }
        }
        
        /// <summary>
        /// 指定循环次数连续做几次某事 
        /// </summary>
        public static void Foreach(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            IList<object> list = (IList<object>)parameters[0];
            var mList = list.ToList();
            PseudocodeAction act = (PseudocodeAction) parameters[1];
            string loopId = parameters.GetItem(2).GetStr();

            if (mList.Count > 0)
            {
                if (act != null)
                {
                    var one = mList[0];
                    if (loopId != null)
                    {
                        Pseudocode.vars[loopId] = one;
                    }
                    mList.RemoveAt(0);
                    parameters[0] = mList;
                    Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(act.funcs),
                        () => { Foreach(Pseudocode, funcName, callback, parameters, commonParameters); }, commonParameters);
                }
                else
                {
                    callback?.Invoke(null);
                }
            }
            else
            {
                callback?.Invoke(null);
            }
        }
        
        /// <summary>
        /// 按固定时间周期做某事
        /// </summary>
        public static void Loop(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            int loopCount = parameters[0].GetInt();
            float duration = parameters[1].GetFloat();
            PseudocodeAction LoopDoSth = (PseudocodeAction) parameters[2];
            string loopId = parameters.GetItem(3).GetStr();

            if (!LoopDoSth)
            {
                Debug.LogError("Loop 方法第3个参数只接受NewAction方法返回的参数！");
            }

            CoroutineHelper.Instance.StartCoroutine(LoopCor(Pseudocode, funcName, callback, 
                loopCount, duration, LoopDoSth, loopId, commonParameters));
        }

        private static IEnumerator LoopCor(Pseudocode Pseudocode, string funcName, Action<object> callback, 
            int loopCount, float duration, PseudocodeAction act, string loopId, object[] commonParameters)
        {
            var cur = 0;
            while (cur < loopCount || loopCount == -1)
            {
                yield return new WaitForSeconds(duration);
                if (loopId != null)
                {
                    Pseudocode.vars[loopId] = cur;
                }
                cur++;
                Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(act.funcs), () =>
                {
                    // 单线程处理，不用担心
                }, commonParameters);
            }
            callback?.Invoke(null);
        }

        /// <summary>
        /// 并行处理多个动作，非多线程
        /// </summary>
        public static void Parallel(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParameters)
        { 
            var count = parameters.Length;
            var cur = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                PseudocodeAction act = (PseudocodeAction) parameters[i];
                if (!act)
                {
                    Debug.LogError("Parallel 方法只接受NewAction方法返回的参数！");
                    cur++;
                    if (cur == count)
                    {
                        // 全部并行处理完，再返回
                        callback?.Invoke(null);
                    }
                    continue;
                }
                Pseudocode.RunQueue(Pseudocode.GetFuncParamFuncsQueue(act.funcs), () =>
                {
                    // 单线程处理，不用担心
                    cur++;
                    if (cur == count)
                    {
                        // 全部并行处理完，再返回
                        callback?.Invoke(null);
                    }
                }, commonParameters);
            }
        }
        
        public static void Add(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a + b);
        }
        
        public static void Subtract(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a - b);
        }
        
        public static void Minus(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
        
            callback?.Invoke(- a);
        }
        
        public static void SelfAdd(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
        
            callback?.Invoke(++a);
        }
        
        public static void SelfSubtract(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
        
            callback?.Invoke(--a);
        }
        
        
        public static void Multiply(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a * b);
        }
        
        public static void Divide(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);
        
            callback?.Invoke(a / b);
        }
        
        public static void Remaind(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = GetFloat(parameters[0]);
            float b = GetFloat(parameters[1]);

            callback?.Invoke(a % b);
        }
        
        public static void StringConnect(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(parameters[i]);
            }
            callback?.Invoke(sb.ToString());
        }
        
        public static void Print(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < parameters.Length; i++)
            {
                sb.Append(parameters[i]);
            }
            Debug.Log(sb.ToString());
            callback?.Invoke(null);
        }
        
        public static void And(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            bool a = (bool)parameters[0];
            bool b = (bool)parameters[1];
        
            callback?.Invoke(a && b);
        }
        
        public static void Or(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            bool a = (bool)parameters[0];
            bool b = (bool)parameters[1];
        
            callback?.Invoke(a || b);
        }
        
        public static void Not(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            bool a = (bool)parameters[0];
        
            callback?.Invoke(! a);
        }
        
        public static void RandomRange(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            float a = parameters[0].GetFloat();
            float b = parameters[1].GetFloat();
            bool isInt = parameters[2].GetBool();

            if (isInt)
            {
                callback?.Invoke(Random.Range(Mathf.RoundToInt(a), Mathf.RoundToInt(b)));
            }
            else
            {
                callback?.Invoke(Random.Range(a, b));
            }
        }
        
        public static void toList(Pseudocode Pseudocode, string funcName, Action<object> callback, object[] parameters, object[] commonParameters)
        {
            callback?.Invoke(new []{parameters[0]}.ToList());
        }

    }
}