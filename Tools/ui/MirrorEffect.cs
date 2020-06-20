using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MirrorDirectType
{
    None,
    Horizontal,
    Vertical,
    HorAndVer,
}

[AddComponentMenu("UI/Effects/MirrorEffect")]
public class MirrorEffect : BaseMeshEffect
{
    public MirrorDirectType mirrorDirectType = MirrorDirectType.Horizontal;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }
        if (mirrorDirectType == MirrorDirectType.None)
        {
            return;
        }
        var vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);
        ApplyEffect(vertexList);
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);
    }

    private void ApplyEffect(List<UIVertex> vertexList)
    {
        var pivot = graphic.rectTransform.pivot;
        var size = graphic.rectTransform.sizeDelta;
        for (int i = 0; i < vertexList.Count; i++)
        {
            var uiVertex = vertexList[i];
//            var tempUV = uiVertex.uv0;
            var tempPosition = uiVertex.position;
            switch (mirrorDirectType)
            {
                case MirrorDirectType.Horizontal:
                    //                    tempUV.x = 1 - tempUV.x;
                    tempPosition.x = size.x * (1 - 2* pivot.x) - tempPosition.x;
                    break;
                case MirrorDirectType.Vertical:
//                    tempUV.y = 1 - tempUV.y;
                    tempPosition.y = size.y * (1 - 2 * pivot.y) - tempPosition.y;
                    break;
                case MirrorDirectType.HorAndVer:
//                    tempUV.x = 1 - tempUV.x;
//                    tempUV.y = 1 - tempUV.y;
                    tempPosition.x = size.x * (1 - 2* pivot.x) - tempPosition.x;
                    tempPosition.y = size.y * (1 - 2 * pivot.y) - tempPosition.y;
                    break;
            }
            //            uiVertex.uv0 = tempUV;
            uiVertex.position = tempPosition;
            vertexList[i] = uiVertex;
        }
    }
}
