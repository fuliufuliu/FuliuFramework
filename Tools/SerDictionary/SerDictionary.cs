using System;
using System.Collections.Generic;
using UnityEngine;

namespace fuliu.serialize
{
    /// <summary>
    /// 此字段单独使用虽然可以被系列化，但作为某对象的字段时，因为是泛型而不能系列化
    /// 所以必须再这个类的基础上包装一层，如 IntStringDic, 消除两个泛型变为普通类型
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable] 
    public abstract class SerDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] 
        List<TKey> _keys; 

        [SerializeField] 
        List<TValue> _values; 

        public void OnBeforeSerialize() 
        { 
            _keys = new List<TKey>(Keys); 
            _values = new List<TValue>(Values); 
        } 

        public void OnAfterDeserialize() 
        { 
            var count = Math.Min(_keys.Count, _values.Count); 
            Clear();
            for (var i = 0; i < count; ++i) 
            { 
                Add(_keys[i], _values[i]); 
            } 
        } 
    }
}