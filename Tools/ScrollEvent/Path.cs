using System.Collections.Generic;
using UnityEngine;

namespace ScrollEvent
{
    public class Path
    {
        private List<Vector3> posKeys;

        public Path(List<Transform> path)
        {
            posKeys = new List<Vector3>(path.Count);

            for (int i = 0; i < path.Count; i++)
            {
                posKeys.Add(path[i].position);
            }
        }
        
        public Path(List<Vector3> path)
        {
            posKeys = new List<Vector3>(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                posKeys.Add(path[i]);
            }
        }

       
        /// <summary>
        /// scale 是整条path的比例 [0, n], n是 path中线段数量
        /// </summary>
        public Vector3 GetPos2(float scale)
        {
            var transStartIndex = (int) scale;
            transStartIndex = Mathf.Clamp(transStartIndex, 0, posKeys.Count - 2);
            var _scale = scale - transStartIndex;
            return Vector3.Lerp(posKeys[transStartIndex], posKeys[transStartIndex + 1], _scale);
        }
    }
}