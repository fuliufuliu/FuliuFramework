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
        /// key:定义的原始消息，value：组合的消息(key:id 或GameObject对应的GetInstanceID，value：组合Id)
        /// </summary>
        static Dictionary<string, Dictionary<int,string>> _msgKeyDic = new Dictionary<string, Dictionary<int, string>>();
        /// <summary>
        /// key:定义的原始消息，value：组合的消息(key: 某个接收消息的 组件，value：接收消息的方法)
        /// </summary>
        static Dictionary<string, Dictionary<Component, CallFun>> componentCallFuncDic = new Dictionary<string, Dictionary<Component, CallFun>>();
        /// <summary>
        /// key：组合的消息 ，value：调用的方法
        /// </summary>
        static Dictionary<string, CallFun> _msgDic = new Dictionary<string, CallFun>();
        /// <summary>
        /// 事件缓存
        /// </summary>
        private static List<CallFun> eventListTemp = new List<CallFun>(20);
    
        private static List<CallFun> eventList = new List<CallFun>(8);
        
        public static Action<string> onLogError { get; set; }
        public static Action<string> onLogWarning { get; set; }

        static CsMessageManager()
        {
            onLogError = Debug.LogError;
            onLogWarning = Debug.LogWarning;
        }
    
        /// <summary>
        /// 监听消息，CallFunNull不回调参数  CallFun带一个参数 CallFun2 带2个参数
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="fun"></param>
        public static void addMessage(string msg, CallFun fun, GameObject target=null,int id=100000)
        {
    		int targetId=target== null ?  id : target.GetInstanceID();
    		var msgEx = msg +"#"+ targetId;
            if (!_msgKeyDic.ContainsKey(msg)) {
                _msgKeyDic[msg] = new Dictionary<int, string>();
            }
            if (_msgKeyDic.ContainsKey(msg) && _msgKeyDic[msg].ContainsKey(targetId)) {
                onLogWarning?.Invoke(msgEx + "可能没有删!");
            }
            _msgKeyDic[msg][targetId] = msgEx;
            _msgDic[msgEx] = fun;
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
        
        public static void addMessage(string msg, CallFun fun, MonoBehaviour target=null,int id=100000)
        {
            int targetId=target== null ?  id : target.GetInstanceID();
            var msgEx = msg +"#"+ targetId;
            if (!_msgKeyDic.ContainsKey(msg)) {
                _msgKeyDic[msg] = new Dictionary<int, string>();
            }
            if (_msgKeyDic.ContainsKey(msg) && _msgKeyDic[msg].ContainsKey(targetId)) {
                onLogWarning?.Invoke(msgEx + "可能没有删!");
            }
            _msgKeyDic[msg][targetId] = msgEx;
            _msgDic[msgEx] = fun;
        }
    
        /// <summary>
        ///  发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sender"></param>
        public static void sendMessage(string msg, params object[] senders)
        {
            if (_msgKeyDic.ContainsKey(msg))
            {
                var keys = _msgKeyDic[msg];
                eventList.Clear();
                foreach (var keyPair in keys)
                {
                    if (_msgDic.ContainsKey(keyPair.Value))
                    {
                        eventList.Add(_msgDic[keyPair.Value]);
                    }
                }
    
                if (eventList.Count > 0)
                {
                    for (int i = 0; i < eventList.Count; i++)
                    {
                        try
                        {
                            if (eventList[i] != null)
                            {
                                eventList[i](senders);
                            }
                        }
                        catch (Exception ex)
                        {
                            onLogError?.Invoke("CsMessageManager::::" + ex);
                        }
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
    	public static void remove(string msg, GameObject target=null,int id=100000) {
            int targetId = target == null ? id : target.GetInstanceID();
    		var msgEx = msg +"#"+ targetId;
            _msgDic.Remove(msgEx);
            if (_msgKeyDic.ContainsKey(msg)) {
                _msgKeyDic[msg].Remove(targetId);
            }
        }
        
        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isBubb"></param>
        public static void remove(string msg, MonoBehaviour target=null,int id=100000) {
            int targetId = target == null ? id : target.GetInstanceID();
            var msgEx = msg +"#"+ targetId;
            _msgDic.Remove(msgEx);
            if (_msgKeyDic.ContainsKey(msg)) {
                _msgKeyDic[msg].Remove(targetId);
            }
        }
        
    	public static void removeAll()
    	{
            _msgDic.Clear();
            _msgKeyDic.Clear();
        }
        /// <summary>
        /// 根据组合的msgEx分离得到原始msg
        /// </summary>
        /// <param name="msgEx">组合的msgEx</param>
    	private static string getMessage(string msgEx)
    	{
    		char[] split={'#'};
    		return msgEx.Split(split)[0];
    	}
        /// <summary>
        /// 根据组合的msgEx分离得到调用的方法
        /// </summary>
    	private static CallFun getFun(string msgEx) {
    	    if (_msgDic.ContainsKey(msgEx)) {
    	        return _msgDic[msgEx];
    	    }
    		return null;
    	}
    
    	/// <summary>
    	/// 判断是否存在该监听
    	/// </summary>
    	public static bool hasMessage(string msgEx) {
    	    return _msgDic.ContainsKey(msgEx);
    //        return _mapList.hasMap(msg);
    	}
    
        protected void OnDestroy() {
            _msgDic = null;
            _msgKeyDic = null;
        }
    }
}

