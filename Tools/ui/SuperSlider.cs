using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SuperSlider : Slider
{
    public SliderEvent onDraging;

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        onDraging.Invoke(eventData.clickTime);
    }
}   
