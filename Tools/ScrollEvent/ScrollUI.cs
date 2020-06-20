using UnityEngine;
using UnityEngine.EventSystems;

namespace ScrollEvent
{
    public class ScrollUI : Scroll
    {
        protected override void Update()
        {
            if (Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject(0) || 
                Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() )
            {
                if (! isTouched)
                {
                    // 刚按下
                    StartDrag();
                }
                else
                {
                    // 按下后每帧更新
                    Drag();
                }
                isSpringed = false;
                isTouched = true;
            }
            else
            {
                if (isTouched)
                {
                    // 刚松手
                    StopDrag();
                }
                else
                {
                    // 松手后每帧更新
                    if (! isSpringed)
                    {
                        Spring();
                    }
                }
                isTouched = false;
            }
        }
    }
}