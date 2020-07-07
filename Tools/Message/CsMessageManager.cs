using System;
using System.Collections.Generic;
using System.Linq;
using _Game;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CsMessage
{
   
    public delegate void CallFun(params object[] senders);
    
    public class CsMessageManager {

        /// <summary>
        /// key1:定义的原始消息，key2: 某个接收消息的 组件，value：事件
        /// </summary>
        static Dictionary<string, Dictionary<Component, CallFun>> componentCallFuncDic = new Dictionary<string, Dictionary<Component, CallFun>>();

        /// <summary>
        /// key1:定义的原始消息，key2: 某个接收消息的 对象的ID，value：事件
        /// </summary>
        static Dictionary<string, Dictionary<int, CallFun>> msgIdEventDic = new Dictionary<string, Dictionary<int, CallFun>>();
        
        public static Action<string> onLogError { get; set; }
        public static Action<string> onLogWarning { get; set; }

        static CsMessageManager()
        {
            onLogError = Debug.LogError;
            onLogWarning = Debug.LogWarning;
        }
    
        public static void addHierarchyMessage(string msg, CallFun fun, Component trans)
        {
            if (! componentCallFuncDic.ContainsKey(msg)) 
                componentCallFuncDic[msg] = new Dictionary<Component, CallFun>();
            if (!componentCallFuncDic[msg].ContainsKey(trans))
                componentCallFuncDic[msg][trans] = fun;
        }
        
        public static void removeHierarchyMessage(string msg, Component trans)
        {
            if (componentCallFuncDic.ContainsKey(msg))
                if (componentCallFuncDic[msg].ContainsKey(trans))
                    componentCallFuncDic[msg].Remove(trans);
        }
        
        public static void addMessage(string msg, CallFun fun, MonoBehaviour target=null, int id=100000)
        {
            int targetId = target == null ?  id : target.GetInstanceID();
            if (! msgIdEventDic.ContainsKey(msg))
            {
                msgIdEventDic[msg] = new Dictionary<int, CallFun>();
            }

            if (msgIdEventDic[msg].ContainsKey(targetId))
            {
                onLogWarning?.Invoke($"{msg} # {targetId} 可能没有删!");
            }
            msgIdEventDic[msg][targetId] = fun;
        }
    
        /// <summary>
        ///  发送消息
        /// </summary>
        public static void sendMessage(string msg, params object[] senders)
        {
            if (msgIdEventDic.ContainsKey(msg))
            {
                var idEvents = msgIdEventDic[msg];
                foreach (var idEventPair in idEvents)
                {
                    try
                    {
                        idEventPair.Value(senders);
                    }
                    catch (Exception ex)
                    {
                        onLogError?.Invoke("CsMessageManager::::" + ex);
                    }
                }
            }
        }
        
        
        public static void sendToParentMessage(Transform myTransform, string msg, params object[] paramData)
        {
            if (componentCallFuncDic.ContainsKey(msg))
            {
                var parents = GetParents(myTransform);
                if (parents.Count > 0)
                {
                    foreach (var pair in componentCallFuncDic[msg])
                    {
                        var parentComp = pair.Key;
                        if (parentComp && parents.Contains(parentComp.transform))
                        {
                            try
                            {
                                pair.Value.Invoke(paramData);
                            }
                            catch (Exception ex)
                            {
                                onLogError?.Invoke("CsMessageManager::::" + ex);
                            }
                        }
                    }
                }
            }
        }

        private static List<Transform> GetParents(Transform transform)
        {
            var res = new List<Transform>();
            while (transform.parent)
            {
                res.Add(transform.parent);
                transform = transform.parent;
            }
            return res;
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isBubb"></param>
        public static void remove(string msg, MonoBehaviour target=null,int id=100000) {
            int targetId = target == null ? id : target.GetInstanceID();
            if (msgIdEventDic.ContainsKey(msg))
            {
                if (msgIdEventDic[msg].ContainsKey(targetId))
                {
                    msgIdEventDic[msg].Remove(targetId);
                }
            }
        }
        
    	public static void removeAll()
    	{
            msgIdEventDic.Clear();
        }
        
    	/// <summary>
    	/// 判断是否存在该监听
    	/// </summary>
    	public static bool hasMessage(string msgEx) {
    	    return msgIdEventDic.ContainsKey(msgEx) && msgIdEventDic[msgEx].Count > 0;
    	}
        
        public static bool hasMessage(string msgEx, MonoBehaviour target=null,int id=100000) {
            int targetId = target == null ? id : target.GetInstanceID();
            return msgIdEventDic.ContainsKey(msgEx) && msgIdEventDic[msgEx].ContainsKey(targetId);
        }
    
        protected void OnDestroy()
        {
            msgIdEventDic = null;
            componentCallFuncDic = null;
        }
    }
}

