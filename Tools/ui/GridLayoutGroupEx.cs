#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Grid Layout Group Ex", 153)]
public class GridLayoutGroupEx : GridLayoutGroup
{
#if UNITY_EDITOR
    [MenuItem("Tools/GridLayoutGroupEx/Change To GridLayoutGroup")]
    public static void ChangeTo_GridLayoutGroup()
    {
        var go = (GameObject)Selection.activeObject;
        if (go)
        {
            var oComponent = go.GetComponent<GridLayoutGroupEx>();
            if (oComponent)
            {
                var goTemp = Instantiate(go);
                Undo.DestroyObjectImmediate(oComponent);
                var nComponent = Undo.AddComponent<GridLayoutGroup>(go);

                copyArributes(goTemp.GetComponent<GridLayoutGroup>(), nComponent);
                DestroyImmediate(goTemp);
            }
        }
    }
    [MenuItem("Tools/GridLayoutGroupEx/Change To GridLayoutGroupEx")]
    public static void ChangeTo_GridLayoutGroupEx()
    {
        var go = (GameObject)Selection.activeObject;
        if (go)
        {
            var oComponent = go.GetComponent<GridLayoutGroup>();
            if (oComponent)
            {
                var goTemp = Instantiate(go);
                Undo.DestroyObjectImmediate(oComponent);
                var nComponent = Undo.AddComponent<GridLayoutGroupEx>(go);

                copyArributes(goTemp.GetComponent<GridLayoutGroup>(), nComponent);
                DestroyImmediate(goTemp);
            }
        }
    }

#else
    public static void ChangeTo_GridLayoutGroup()
    {
        if (!Application.isEditor) throw new Exception("ChangeTo_GridLayoutGroup 方法只能在Unity Editor下调用！");
    }

    public static void ChangeTo_GridLayoutGroupEx()
    {
        if (!Application.isEditor) throw new Exception("ChangeTo_GridLayoutGroupEx 方法只能在Unity Editor下调用！");
    }
#endif


    public static void copyArributes(GridLayoutGroup @from, GridLayoutGroup to)
    {
        if (to && @from)
        {
            to.padding = @from.padding;
            to.cellSize = @from.cellSize;
            to.spacing = @from.spacing;
            to.startCorner = @from.startCorner;
            to.startAxis = @from.startAxis;
            to.childAlignment = @from.childAlignment;
            to.constraint = @from.constraint;
            to.constraintCount = @from.constraintCount;
        }
    }

    /// <summary>
    /// <para>
    /// 修正了GridLayoutGroup 固定ConstraintCount时，宽度不会随着元素减小而变小
    /// </para>
    /// </summary>
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        int num1;
        int num2;
        if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            var count = Mathf.Max(Mathf.Min(this.m_ConstraintCount, this.rectChildren.Count), 1f);
            num2 = num1 = Mathf.CeilToInt(count);
        }
        else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            num2 = num1 = Mathf.CeilToInt((float) ((double) this.rectChildren.Count/(double)this.m_ConstraintCount - 1.0/1000.0));
        }
        else
        {
            num2 = 1;
            num1 = Mathf.CeilToInt(Mathf.Sqrt((float) this.rectChildren.Count));
        }
        var totalMin = (float) this.padding.horizontal + (this.cellSize.x + this.spacing.x)*(float) num2 -
                       this.spacing.x;
        var totalPreferred = (float)this.padding.horizontal + (this.cellSize.x + this.spacing.x)*(float) num1 - this.spacing.x;
        this.SetLayoutInputForAxis(totalMin, totalPreferred, -1f, 0);
    }


    /// <summary>
    /// <para>
    /// Called by the layout system.
    /// </para>
    /// </summary>
    public override void CalculateLayoutInputVertical()
    {
        float num = (float)this.padding.vertical + 
            (this.cellSize.y + this.spacing.y) * (
                this.m_Constraint != GridLayoutGroup.Constraint.FixedColumnCount ? (
                    this.m_Constraint != GridLayoutGroup.Constraint.FixedRowCount ? 
                        (float)Mathf.CeilToInt((float)this.rectChildren.Count / (float)Mathf.Max(1, Mathf.FloorToInt((float)(((double)this.rectTransform.rect.size.x - (double)this.padding.horizontal + (double)this.spacing.x + 1.0 / 1000.0) / ((double)this.cellSize.x + (double)this.spacing.x))))) 
                        : (float)this.m_ConstraintCount) 
                    : (float)Mathf.CeilToInt((float)((double)this.rectChildren.Count / (double)this.m_ConstraintCount - 1.0 / 1000.0))
                ) - this.spacing.y;
        this.SetLayoutInputForAxis(num, num, -1f, 1);
    }
}
