using System.Collections.Generic;
using UnityEngine;

namespace CsMessage
{
    public abstract class MsgMonoBehaviour : MonoBehaviour
    {
        protected bool inited;

        public virtual void Init()
        {
            if (! inited)
            {
                inited = true;
            }
        }
        
        Dictionary<string, CallFun> msgDic = new Dictionary<string, CallFun>();
        Dictionary<string, CallFun> msgHierarchyDic = new Dictionary<string, CallFun>();

        protected void AddMsg(string msg, CallFun fun)
        {
            CsMessageManager.addMessage(msg, fun, this);
            msgDic[msg] = fun;
        }
        
        /// <summary>
        /// 添加层级上下层的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="fun"></param>
        protected void AddHierarchyMsg(string msg, CallFun fun)
        {
            CsMessageManager.addHierarchyMessage(msg, fun, this);
            msgHierarchyDic[msg] = fun;
        }

        void RemoveAllMsg()
        {
            foreach (var pair in msgDic)
            {
                CsMessageManager.remove(pair.Key, this);
            }
            
            foreach (var pair in msgHierarchyDic)
            {
                CsMessageManager.removeHierarchyMessage(pair.Key, this);
            }
        }

        protected virtual void OnDestroy()
        {
            RemoveAllMsg();
        }
    }
}