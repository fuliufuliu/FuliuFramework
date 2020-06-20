using System;
using fuliu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.UI
{
    public class JoystickUI : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Tooltip("是否固定摇杆的位置，若不固定，那么点击TouchArea上的任意位置，摇杆的初始位置就会设定在点击位置上")]
        public bool isFixJoyPos;
        public Camera uiCamera;
        private RectTransform simpleTouch;
        private RectTransform area;
        private Vector3 globalMousePos;
        private RectTransform joystick;
        [NonSerialized]
        public Vector3 startPos;
        [NonSerialized]
        public Vector3 dragOffset;
        [NonSerialized]
        public Vector3 dragDeltaOffset;
        public Action<Vector3> onTouchDown;
        public Action<Vector3> onBeginDrag;
        public Action onEndDrag;
        public Action<Vector3, Vector3> onDrag;
        public Action onTouchUp;
        [Tooltip("摇杆的中心是否跟随拖动的点移动，当拖动超过摇杆的最大距离后，中心也跟着移动到最大距离允许的范围内")]
        public bool isJoyCenterFollowTouchPoint = true;
        [Tooltip("摇杆的中心跟随拖动的点移动的最大距离")]
        float JoyCenterFollowMaxDistance = 1.50f; 
 
        private void Awake()
        {
            simpleTouch = transform.Find("SimpleTouch") as RectTransform;
            joystick = simpleTouch.transform.Find("Joystick") as RectTransform;
            area = transform.Find("TouchArea") as RectTransform;
            startPos = joystick.transform.position;
        }

        public void OnTouchDown()
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(area, Input.mousePosition, uiCamera, out globalMousePos);
            if (isFixJoyPos)
            {
                dragDeltaOffset = dragOffset = globalMousePos - startPos;
            }
            else
            {
                simpleTouch.transform.position = globalMousePos;
                dragOffset = startPos = globalMousePos;
            }
            onTouchDown?.Invoke(startPos);
        }
        
        public void OnTouchUp()
        {
            onTouchUp?.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(startPos);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            joystick.transform.localPosition = Vector3.zero;
            onEndDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(area, Input.mousePosition, uiCamera, out globalMousePos);
            if (isJoyCenterFollowTouchPoint)
            {
//                print("Touch Distance" + Vector3.Distance(globalMousePos, startPos));
                startPos = UnityTools.GetFollowPoint(globalMousePos, startPos, JoyCenterFollowMaxDistance);
            }
            dragDeltaOffset = globalMousePos - joystick.transform.position; 
            joystick.transform.position = globalMousePos;
            dragOffset = globalMousePos - startPos;
            onDrag?.Invoke(dragOffset, dragDeltaOffset);
        }
    }
}