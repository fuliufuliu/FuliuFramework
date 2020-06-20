using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class PoolTool
{
    /// <summary>
    /// 填充 Transform 每个孩子的内容，如果孩子不够，会生孩子，孩子多了会隐藏孩子，孩子可重复利用。
    /// 默认parent的第一个孩子（序号0）为蒙板来复制。蒙板会被隐藏，所以不要使用需要显示的内容为孩子的蒙板。
    /// </summary>
    /// <param name="parent">孩子的爹</param>
    /// <param name="datas">孩子的数据</param>
    /// <param name="itemTemplet">孩子的蒙板</param>
    /// <typeparam name="TData">这个孩子的数据</typeparam>
    public static void TransformChildLoop<TData>(Transform parent, List<TData> datas, Action<Transform, TData, int> onUpdateItem,
        Transform itemTemplet = null)
    {
        if (parent)
        {
            var firstChildIndex = 0;
            if (itemTemplet == null)
            {
                firstChildIndex = 1;

                if (parent.childCount > 0)
                {
                    itemTemplet = parent.GetChild(0);
                }
                else
                {
                    Debug.LogError($"itemTemplet 参数为null了并且未在parent下找到可用的蒙板item！");
                    return;
                }
            }
            else if(itemTemplet.parent == parent)
            {
                firstChildIndex = 1;
            }
            else
            {
                firstChildIndex = 0;
            }

            if (itemTemplet == null)
            {
                return;
            }
            
            itemTemplet.gameObject.SetActive(false);

            var childOldCount = parent.childCount - firstChildIndex;
            if (childOldCount < datas.Count)
            {
                for (int i = childOldCount; i < datas.Count; i++)
                {
                    var child = Object.Instantiate(itemTemplet).transform;
                    child.SetParent(parent);
                    Reset(child); 
                }
            }
            else if (childOldCount > datas.Count)
            {
                for (int i = datas.Count; i < childOldCount; i++)
                {
                    parent.GetChild(i + firstChildIndex).gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < datas.Count; i++)
            {
                var child = parent.GetChild(i + firstChildIndex);
                child.gameObject.SetActive(true);
                onUpdateItem?.Invoke(child, datas[i], i);
            }
        }
        else
        {
            Debug.LogError($"parent 参数为null了!");
        }
    }

    public static void Reset(this Transform child)
    {
        child.localPosition = Vector3.zero;
        child.localRotation = Quaternion.identity;
        child.localScale = Vector3.one;
    }

    public static void TransformChildLoop(Transform parent, int dataCount,
        Action<Transform, int> onUpdateItem,
        Transform itemTemplet = null)
    {
        TransformChildLoop(parent, Enumerable.Range(0, dataCount).ToList(),
            (child, index, _)=>
            {
                onUpdateItem?.Invoke(child, index);
            }, 
            itemTemplet);
    }
    
    /// <summary>
    /// 遍历所有子对象（倒序遍历），要求node有 numChildren 属性获取子节点的数量和 getChildAt(i) 方法获取子节点
    /// isOneLevel 是否只遍历亲代（儿女，非孙辈）子节点，
    /// func 遍历过程中找到每个子节点后会调用  func(_this, newChild) ，对每个子节点进行处理。
    /// </summary>
    public static void EnumAllChild(this Transform node, bool isOneLevel, Action<Transform> func){
        if (node && node.childCount > 0){
            for (var i = node.childCount - 1; i >= 0; i --){
                var newChild = node.GetChild(i);
                func(newChild);
                if(! isOneLevel){
                    EnumAllChild(newChild, isOneLevel, func);
                }
            }
        }
    }

    /// <summary>
    /// 遍历所有子对象（正序遍历），要求node有 numChildren 属性获取子节点的数量和 getChildAt(i) 方法获取子节点
    ///   isOneLevel 是否只遍历亲代（儿女，非孙辈）子节点，
    ///  func 遍历过程中找到每个子节点后会调用  func(_this, newChild) ，对每个子节点进行处理。
    /// </summary>
    public static void EnumAllChild2(this Transform node, bool isOneLevel, Action<Transform> func){
        if (node && node.childCount > 0){
            for (var i = 0; i < node.childCount ; i ++){
                var newChild = node.GetChild(i);
                func(newChild);
                if(! isOneLevel){
                    EnumAllChild(newChild, isOneLevel, func);
                }
            }
        }
    }
    
    public static Transform GetRandomChild(this Transform node){
        if (node && node.childCount > 0){
            return node.GetChild(Random.Range(0, node.childCount));
        }
        return null;
    }

}
