// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.UI;
//
// public static class SkinLoader
// {
//     public static void SetSkin2DTexture(RawImage skinImage, string skinDataAssetName)
//     {
//         TextureManager.SetTexture(skinImage, CommonPath.skinIconPath + skinDataAssetName);
//     }
//
//     
//     public static async Task<GameObject> GetSkin(SkinData_Row skinData)
//     {
//         if (! GoPoolManager.Instance.HasPool(skinData.assetName))
//         {
// //            var prefab = LoadManager.LoadAsync<GameObject>(CommonPath.skinPrefabPath + skinData.assetName).Result;
//             var prefab = await LoadManager._LoadAsync<GameObject>(CommonPath.skinPrefabPath + skinData.assetName);
//             if (prefab)
//             {
//                 GoPoolManager.Instance.CreatePool(skinData.assetName, prefab);
//             }
//             else
//             {
//                 throw new Exception($"未加载到皮肤！{skinData.assetName}");
//             }
//         }
//
//         return GoPoolManager.Instance.Pop(skinData.assetName);
//     }
//
//     public static async Task SetSkin(Transform parent, SkinData_Row skinData)
//     {
//         if (parent)
//         {
//             var skin = await GetSkin(skinData);
//             if (skin)
//             {
//                 skin.name = skinData.assetName;
// //                restoreOldSkin:
//                 if (parent.childCount > 0)
//                 {
//                     var oldSkin = parent.GetChild(0);
//                     GoPoolManager.Instance.Push(oldSkin.name, oldSkin.gameObject);
// //                    goto restoreOldSkin;
//                 }
//                 
//                 skin.transform.SetParent(parent);
//                 skin.transform.Reset();
//                 skin.gameObject.SetActive(true);
//             }
//         }
//     }
// }
