using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScrollEvent
{
    public class Scroll : MonoBehaviour
    {
        public bool isTouched;
        public bool isSpringed;
        protected Vector2 dragStartPos;
        protected Vector2 lastFrameDragOff;
        protected Vector2 dragOff;
        protected Vector2 deltaDragOff;
        protected Vector2 lastDeltaDragOff;
        public Action onSpring;
        public Action onStopDrag;
        public Action<Vector2, Vector2> onDrag;
        public Action<Vector2> onStartDrag;
        
        // 使用
        // private float projectScale = -0.006f;
        // private float currentScaleValue;
        // private Path cameraPath;
        // private Path lightPath;
        // private float springSpeed = 10;
        // private int lastTransStartIndex = -1;

        protected virtual void Update()
        {
            if (Input.touchCount == 1 && ! EventSystem.current.IsPointerOverGameObject(0) || 
                Input.GetMouseButton(0) && ! EventSystem.current.IsPointerOverGameObject() )
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

        protected void StartDrag()
        {
            if (Input.GetMouseButton(0))
            {
                dragStartPos = Input.mousePosition;
                print("开始时：Input.mousePosition" + Input.mousePosition);
            }
            else
            {
                dragStartPos = Input.touches[0].position;
            }
            lastFrameDragOff = dragStartPos;

            onStartDrag?.Invoke(dragStartPos);
        }

        protected void Drag()
        {
            if (Input.GetMouseButton(0))
            {
                var mousePosition = (Vector2) Input.mousePosition;
                // print("Drag：mousePosition" + mousePosition);
                dragOff = mousePosition - dragStartPos;
                deltaDragOff = mousePosition - lastFrameDragOff;
                lastFrameDragOff = mousePosition;
            }
            else
            {
                var touchPos = Input.touches[0].position;
                dragOff = touchPos - dragStartPos;
                deltaDragOff = touchPos - lastFrameDragOff;
                lastFrameDragOff = touchPos;
            }

            lastDeltaDragOff = deltaDragOff;
            
            onDrag?.Invoke(dragOff, deltaDragOff);
        }
        protected void StopDrag()
        {
            onStopDrag?.Invoke();
        }
        protected void Spring()
        {
            onSpring?.Invoke();
        }
        
        // // 使用案例
        ///////////////////////
        // private void OnStartDrag(Vector2 dragStartPos)
        // {
        // }
        //
        //
        // private void OnDrag(Vector2 dragOff, Vector2 deltaDragOff)
        // {
        //     currentScaleValue += deltaDragOff.x * projectScale;
        //     carema3d.transform.position = cameraPath.GetPos2(currentScaleValue);
        //
        //     var transStartIndex = Mathf.RoundToInt(currentScaleValue);
        //     if (lastTransStartIndex != transStartIndex)
        //     {
        //         ToCarTarget(transStartIndex);
        //     }
        // }
        //         
        // private void OnStopDrag()
        // {
        //     
        // }
        //         
        // private void OnSpring()
        // {
        //     //            惯性
        //     ///////////////////////////////
        //     
        //     // Spring
        //     var transStartIndex = Mathf.RoundToInt(currentScaleValue);
        //    
        //     if (Math.Abs(currentScaleValue - transStartIndex) < 0.001f)
        //     {
        //         isSpringed = true;
        //         currentScaleValue = transStartIndex;
        //         print($"  ToCarTarget -> currentScaleValue  {currentScaleValue} ");
        //         // 刷新数值 
        //         ToCarTarget(transStartIndex);
        //     }
        //     else
        //     {
        //         currentScaleValue = Mathf.Lerp(currentScaleValue, transStartIndex, Time.deltaTime * springSpeed);
        //         carema3d.transform.position = cameraPath.GetPos2(currentScaleValue);
        //     }
        // }
    }
}