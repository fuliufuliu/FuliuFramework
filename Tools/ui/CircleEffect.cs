using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/CircleEffect")]
[RequireComponent(typeof(Image))]
public class CircleEffect : BaseMeshEffect
{
    [SerializeField, Range(0, 1)]
    public float fillPercent = 1;
    [SerializeField, Range(3, 100)]
    public int segements = 10;

    public enum RadiusMode
    {
        min,
        max,
        width,
        height,
    }

    public RadiusMode radiusMode;

    private RectTransform _rectTransform;
    private Image _image;
    private Sprite _overrideSprite;

    public RectTransform rectTransform
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = transform as RectTransform;
            }
            return _rectTransform;
        }
    }

    private Image image
    {
        get
        {
            if (!_image)
            {
                _image = GetComponent<Image>();
            }
            return GetComponent<Image>();
        }
    }

    private Sprite overrideSprite
    {
        get
        {
            return image.overrideSprite;
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        ////////////////通过RectTransform获取矩形宽高，计算出半径
        float tw = rectTransform.rect.width;
        float th = rectTransform.rect.height;
        float diameter = 0;
        switch (radiusMode)
        {
            case RadiusMode.min:
                diameter = Mathf.Min(tw, th);
                break;
            case RadiusMode.max:
                diameter = Mathf.Max(tw, th);
                break;
            case RadiusMode.width:
                diameter = tw;
                break;
            case RadiusMode.height:
                diameter = th;
                break;
        }
        float outerRadius = rectTransform.pivot.x * diameter;

        ///////////////////////
        vh.Clear();
//        Debug.Log(this + "  " +overrideSprite);
        Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
        float uvCenterX = (uv.x + uv.z)*0.5f;
        float uvCenterY = (uv.y + uv.w)*0.5f;
        float uvScaleX = (uv.z - uv.x)/tw;
        float uvScaleY = (uv.w - uv.y)/th;
        //////////////////////////////
        float degreeDelta = (float) (2*Mathf.PI/segements);
        int curSegements = (int) (segements*fillPercent);

        //////////////////////////////计算顶点、指定三角形
        float curDegree = 0;
        UIVertex uiVertex;
        int verticeCount;
        int triangleCount;
        Vector2 curVertice;

        curVertice = Vector2.zero;
        verticeCount = curSegements + 1;
        uiVertex = new UIVertex();
        uiVertex.color = image.color;
        uiVertex.position = curVertice;
        uiVertex.uv0 = new Vector2(curVertice.x*uvScaleX + uvCenterX, curVertice.y*uvScaleY + uvCenterY);
        vh.AddVert(uiVertex);
        var outterVertices = new List<Vector2>();

        for (int i = 1; i < verticeCount; i++)
        {
            float cosA = Mathf.Cos(curDegree);
            float sinA = Mathf.Sin(curDegree);
            curVertice = new Vector2(cosA*outerRadius, sinA*outerRadius);
            curDegree += degreeDelta;

            uiVertex = new UIVertex();
            uiVertex.color = image.color;
            uiVertex.position = curVertice;
            uiVertex.uv0 = new Vector2(curVertice.x*uvScaleX + uvCenterX, curVertice.y*uvScaleY + uvCenterY);
            vh.AddVert(uiVertex);

            outterVertices.Add(curVertice);
        }

        ////////////////////////////VertexHelper是通过AddTriangle接口接受三角形信息
        triangleCount = curSegements*3;
        for (int i = 0, vIdx = 1; i < triangleCount - 3; i += 3, vIdx++)
        {
            vh.AddTriangle(vIdx, 0, vIdx + 1);
        }
        if (fillPercent == 1)
        {
            //首尾顶点相连
            vh.AddTriangle(verticeCount - 1, 0, 1);
        }
    }
}
