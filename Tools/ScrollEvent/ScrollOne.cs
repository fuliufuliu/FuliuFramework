using UnityEngine;
using UnityEngine.EventSystems;

namespace ScrollEvent
{
    public class ScrollOne : Scroll
    {
        public bool canNextOne = true;
        private bool isTouchedLast;
        private bool isOneStart;
        private bool canNextOneLast;
        private bool isCanNextOne;
        private bool isStartDrag;

        protected override void Update()
        {
            if (Application.isMobilePlatform)
            {
                var fingerId = 0;
                if (Input.touchCount == 1)
                {
                    fingerId = Input.touches[0].fingerId;
                    isTouched = !EventSystem.current.IsPointerOverGameObject(fingerId);
                }
                else
                {
                    isTouched = false;
                }
            }
            else
            {
                isTouched = Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject();
            }
                        
            canNextOne = !isTouched && !isSpringed;
            if (! canNextOneLast && canNextOne)
            {
                isCanNextOne = true;
            }
            canNextOneLast = canNextOne;
            
            if (! isSpringed)
            {
                Spring();
            }
            else if (isTouched)
            {
                if (! isTouchedLast && isCanNextOne)
                {
                    // 刚按下
                    StartDrag();
                    isStartDrag = true;
                }
                else if(isStartDrag)
                {
                    // 按下后每帧更新
                    Drag();
                }
            }

            isTouchedLast = isTouched;
        }
        
            
        public void StartSpring(){
            isSpringed = false;
            isStartDrag = false;
            StopDrag();
        }
    }

}