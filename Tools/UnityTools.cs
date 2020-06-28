using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace fuliu
{
    public static class UnityTools
    {
        public static T SafeGetComponent<T>(this Component component) where T : Component
        {
            if (component)
            {
                var t = component.transform.GetComponent<T>();
                if (t)
                {
                    return t;
                }
                else
                {
                    return component.gameObject.AddComponent<T>();
                }
            }

            return null;
        }
        
        public static T GetSafeComponent<T>(this Component component) where T : Component
        {
            return SafeGetComponent<T>(component);
        }

        public static void DestroyAllChildren(this Transform transform)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 获取跟随点的位置。假设一个点跟随着另一个点移动，跟随距离不超过最大值,如果不超过最大值，则跟随的点位置不变。
        /// </summary>
        /// <param name="target">被跟随的点</param>
        /// <param name="followPoint">被动的点</param>
        /// <param name="maxFollowDistance">最大距离</param>
        /// <returns></returns>
        public static Vector3 GetFollowPoint(Vector3 target, Vector3 followPoint, float maxFollowDistance)
        {
            var dirBA = followPoint - target;
            if (dirBA.magnitude <= maxFollowDistance)
            {
                return followPoint;
            }
            return followPoint + dirBA.normalized * maxFollowDistance - dirBA;
        }


        /// <summary>
        /// 获取Bounds的8个顶点位置
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static List<Vector3> GetBounds8Points(this Bounds bounds)
        {
            var points = new List<Vector3>(8);
            var extents = bounds.extents;
            points.Add(bounds.center + new Vector3(extents.x , extents.y, extents.z));
            points.Add(bounds.center + new Vector3(-extents.x , extents.y, extents.z));
            points.Add(bounds.center + new Vector3(extents.x , -extents.y, extents.z));
            points.Add(bounds.center + new Vector3(-extents.x , -extents.y, extents.z));
            points.Add(bounds.center + new Vector3(extents.x , extents.y, -extents.z));
            points.Add(bounds.center + new Vector3(-extents.x , extents.y, -extents.z));
            points.Add(bounds.center + new Vector3(extents.x , -extents.y, -extents.z));
            points.Add(bounds.center + new Vector3(-extents.x , -extents.y, -extents.z));
            return points;
        }
        
        /// <summary>
        /// 判断两个Bounds是否相交，Unity的 Bounds.Intersects方法其实是判断一个Bounds是否包含另一个Bounds，所以不能
        /// 使用它的方法来判断两个Bounds是否相交
        /// 小技巧，假如 BoundsA 很大（相对于BoundsB来说），那么最好 bounds2 填入 BoundsA， bounds1 填入 Bounds
        /// 这样能快些得出结果。
        /// </summary>
        public static bool IsBoundIntersect(Bounds bounds1, Bounds bounds2)
        {
            var bounds1Points = bounds1.GetBounds8Points();
            for (int i = 0; i < 8; i++)
            {
                if (bounds2.Contains(bounds1Points[i]))
                {
                    return true;
                }
            }

            var bounds2Points = bounds2.GetBounds8Points();
            for (int i = 0; i < 8; i++)
            {
                if (bounds1.Contains(bounds2Points[i]))
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 判断两个Bounds是否相交, IsBoundIntersect2 是 IsBoundIntersect的简化，只判断 bounds1 的
        /// 8个顶点是否包含在 bounds2 中
        /// </summary>
        public static bool IsBoundIntersect2(Bounds bounds1, Bounds bounds2)
        {
            var bounds1Points = bounds1.GetBounds8Points();
            for (int i = 0; i < 8; i++)
            {
                if (bounds2.Contains(bounds1Points[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 将字符串 “#FFFFFF” 表示的颜色值转换成Unity的Color
        /// </summary>
        /// <param name="colorString"></param>
        /// <param name="defaultColor"></param>
        /// <returns></returns>
        public static Color ParseColor(string colorString, Color defaultColor = new Color())
        {
            if (ColorUtility.TryParseHtmlString(colorString, out var nowColor))
            {
                return nowColor;
            }
            
            if (! colorString.Contains("#"))
            {
                colorString = "#" + colorString;
            }
            
            if (ColorUtility.TryParseHtmlString(colorString, out var nowColor2))
            {
                return nowColor2;
            }

            return defaultColor;
        }

    }
}