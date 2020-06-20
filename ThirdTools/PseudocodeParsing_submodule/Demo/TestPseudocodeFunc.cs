using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace fuliu.pseudocode
{
    public static class TestPseudocodeFunc
    {
        public static void CloseDoor(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var wait = DefaultFuncs.GetFloat(parameters[0]);
            Debug.Log($"CloseDoor: {wait}");
            CoroutineHelper.Instance.Delay(wait, () =>
            {
                callback?.Invoke(null);
            });
        }
        
        public static void OpenDoor(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var wait = DefaultFuncs.GetFloat(parameters[0]);
            Debug.Log($"OpenDoor: {wait}");
            CoroutineHelper.Instance.Delay(wait, () =>
            {
                callback?.Invoke(null);
            });
        }     
        
        public static void SelectDoor(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var isOpened = (bool)parameters[0];
            Debug.Log($"SelectDoor: {isOpened}");
            CoroutineHelper.Instance.Delay(2, () =>
            {
                callback?.Invoke(2.5f);
            });
        }
        
        public static void OpenLuDeng(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var wait = (int)parameters[0];
            Debug.Log($"OpenLuDeng: {wait}");
            CoroutineHelper.Instance.Delay(wait, () =>
            {
                callback?.Invoke(null);
            });
        }
        
        public static void MurdererToKill(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            Debug.Log($"MurdererToKill: {2}");
            CoroutineHelper.Instance.Delay(2, () =>
            {
                callback?.Invoke(null);
            });
        }
        
        public static void AbnormalKillHuman(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var human = (string)parameters[0];
            
            Debug.Log($"AbnormalKillHuman: {human} Be Killed!");
            CoroutineHelper.Instance.Delay(2, () =>
            {
                callback?.Invoke(null);
            });
        }
        
        public static void RandomHuman(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var set1 = (HashSet<string>)parameters[0];
            var res = set1.ToList()[Random.Range(0, set1.Count)];
            
            Debug.Log($"RandomHuman: {res}");

            CoroutineHelper.Instance.Delay(2, () =>
            {
                callback?.Invoke(res);
            });
        }        
        
        public static void SubtractSet(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var set1 = (HashSet<string>)parameters[0];
            var set2 = (HashSet<string>)parameters[1];
            Debug.Log($"SubtractSet: {2}");

            var res = new HashSet<string>(set1);
            res.ExceptWith(set2);
            
            CoroutineHelper.Instance.Delay(2, () =>
            {
                callback?.Invoke(res);
            });
        }
         
        public static void GetAllHumans(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var count = (int)parameters[0];
            Debug.Log($"GetAllHumans: {count}");
            
            var res = new HashSet<string>(){"100001", "100002", "100008", "100010"};
            
            CoroutineHelper.Instance.Delay(2, () =>
            {
                callback?.Invoke(res);
            });
        }       
                
        public static void NewSet(Pseudocode Pseudocode, string funcName, Action<object> callback,
            object[] parameters, object[] commonParams)
        {
            var str = (string)parameters[0];
            var item = str.Replace("{", "").Replace("}", "");
            
            Debug.Log($"NewSet: {item}");
            var res = new HashSet<string>() {item};
            CoroutineHelper.Instance.Delay(2, () =>
            {
                callback?.Invoke(res);
            });
        }
    }
}