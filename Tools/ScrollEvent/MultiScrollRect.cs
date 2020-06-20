using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScrollEvent
{
    public class MultiScrollRect : ScrollRect
    {
        // public override void OnScroll(PointerEventData data)
        // {
        //     if (!IsActive())
        //         return;
        //     // this.EnsureLayoutHasRebuilt();
        //     UpdateBounds();
        //     Vector2 scrollDelta = data.scrollDelta;
        //     scrollDelta.y *= -1f;
        //     if (vertical && !horizontal)
        //     {
        //         if (Mathf.Abs(scrollDelta.x) >  Mathf.Abs(scrollDelta.y))
        //             scrollDelta.y = scrollDelta.x;
        //         scrollDelta.x = 0.0f;
        //     }
        //     if (horizontal && !vertical)
        //     {
        //         if (Mathf.Abs(scrollDelta.y) > Mathf.Abs(scrollDelta.x))
        //             scrollDelta.x = scrollDelta.y;
        //         scrollDelta.y = 0.0f;
        //     }
        //     if (data.IsScrolling())
        //         this.m_Scrolling = true;
        //     Vector2 position = this.m_Content.anchoredPosition + scrollDelta * this.m_ScrollSensitivity;
        //     if (this.m_MovementType == MovementType.Clamped)
        //         position += this.CalculateOffset(position - this.m_Content.anchoredPosition);
        //     SetContentAnchoredPosition(position);
        //     UpdateBounds();
        // }
    }
}