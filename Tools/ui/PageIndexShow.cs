using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 此类作为ScrollView的翻页页数进度显示
/// </summary>
public class PageIndexShow : MonoBehaviour
{
    private ToggleGroup toggleGroup;
    private bool inited;
    private int pageCount;

    private void Init()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        inited = true;
    }

    public void Refresh(int pageCount, int index)
    {
        if (! inited)
        {
            Init();
        }

        if (this.pageCount != pageCount)
        {
            UpdateToggleCount(pageCount);
        }
        this.pageCount = pageCount;

        Refresh(index);
    }
    
    public void Refresh(int index) 
    {
        if (! inited)
        {
            Init();
        }
        index = Mathf.Clamp(index, 0, pageCount - 1);

        toggleGroup.SetAllTogglesOff();
        transform.GetChild(index + 1).GetComponent<Toggle>().isOn = true;
    }

    private void UpdateToggleCount(int pageCount)
    {
        PoolTool.TransformChildLoop(transform, pageCount, (child, i) => { child.GetComponent<Toggle>().isOn = false; });
    }
}
