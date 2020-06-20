////作者: 蒙占志 日期: 2016-7-19
////作用: UI基类和mgr功能， 负责UI创建排序回收与lua交互等功能
//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Collections;
//using LuaInterface;
//using LuaFramework;
//
//public class UIBase : LuaBehaviour
//{
//    private const int delta = 10;
//    public bool beKeepDepth = true;
//    public bool beUseLua = true;
//    public bool beUnique = true;
//
//    protected static LinkedList<UIBase> uiList = new LinkedList<UIBase>();
//    protected static LinkedList<UIBase> noDepthList = new LinkedList<UIBase>();
//    protected static LinkedList<UIBase> gcList = new LinkedList<UIBase>();
//    protected static int maxDepth2D = 1;
//    private static int blockCount = 0;
////    protected UIPanel[] panels = null;
////    protected UIPanel panel = null;
//    public override void OnInit(AssetBundle bundle = null, string text = null, bool isAutoClearMemory = true, LuaTable tb = null)
//    {
//        base.OnInit(bundle, text, isAutoClearMemory, tb);
//        //RefreshList(false);
//    }
//
////    public void RefreshList(bool needPanel)
////    {
////        beKeepDepth = needPanel;
////        if (!beKeepDepth)
////        {
////            panel = gameObject.GetComponent<UIPanel>();
////            panels = gameObject.GetComponentsInChildren<UIPanel>(true);
////            Array.Sort<UIPanel>(panels, (p1, p2) => { return p1.depth - p2.depth; });
////            AddToList();
////        }
////        else
////        {
////            noDepthList.AddLast(this);
////        }
////    }
//
//    protected virtual void AddToList()
//    {
//        AddToList(uiList, ref maxDepth2D);
//    }
//
//    protected virtual void RemoveFromList()
//    {
//        RemoveFromList(uiList, ref maxDepth2D, 1);
//    }
//
//    protected void AddToList(LinkedList<UIBase> list, ref int depth)
//    {
//        if (beKeepDepth)
//        {
//            return;
//        }
//
//        if (list.Count > 0)
//        {
//#if UNITY_EDITOR
//            if (list.Find(this) != null)
//            {
//                Debugger.LogError("UI {0} already in ui list", name);
//                return;
//            }
//#endif
//        }
//
//        depth = SetDepth(depth) + delta;
//        list.AddLast(this);
//    }
//
//    protected void RemoveFromList(LinkedList<UIBase> list, ref int depth, int beginDepth)
//    {
//        if (beKeepDepth || list.Count == 0)
//        {
//            noDepthList.Remove(this);
//            return;
//        }
//
//        list.Remove(this);
//        depth = beginDepth;
//
//        var iter = list.GetEnumerator();
//
//        while (iter.MoveNext())
//        {
//            depth = iter.Current.SetDepth(depth) + delta;
//        }
//    }
//
//    int SetDepth(int value)
//    {
//        return value;
////        int baseLine = panels[0].depth;
////        value -= baseLine;
////
////        for (int i = 0; i < panels.Length; i++)
////        {
////            panels[i].depth += value;
////        }
////
////        return panels[panels.Length - 1].depth;
//    }
//
//    protected override void OnDestroy()
//    {
//        RemoveFromList();
//        base.OnDestroy();
//    }
//}
