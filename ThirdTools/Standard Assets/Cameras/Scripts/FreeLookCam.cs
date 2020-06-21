using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.Cameras
{

	
	
    public class FreeLookCam : PivotBasedCameraRig
    {
	    public enum SmoothingMode
	    {
		    None,
		    Smoothing,
		    TurnSmoothingCtrl,
	    }
	    
	    public Transform CameraTrans; // the transform of the camera

        // This script is designed to be placed on the root object of a camera rig,
        // comprising 3 gameobjects, each parented to the next:

        // 	Camera Rig
        // 		Pivot
        // 			Camera

        [SerializeField] private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
        [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
        [SerializeField] private float m_TurnSmoothing;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
        [SerializeField] private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
        [SerializeField] private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
        [SerializeField] private bool m_LockCursor;                   // Whether the cursor should be hidden and locked.
        [SerializeField] private bool m_VerticalAutoReturn;           // set wether or not the vertical axis should auto return

        private float m_LookAngle;                    // The rig's y axis rotation.
        private float m_TiltAngle;                    // The pivot's x axis rotation.
//        private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
		private Vector3 m_PivotEulers;
		private Quaternion m_PivotTargetRot;
		private Quaternion m_TransformTargetRot;
//        public bool isNeedMouseButtonDown = true;
        [NonSerialized]
        public bool CanRotate = true;

        [NonSerialized]
        public  bool isGyroEnabled = true;
        /// <summary>
        /// key: fingerId; value: 是否点中ui
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, bool> fingerTouchDic = new Dictionary<int, bool>(5);
        List<int> touchsTemp = new List<int>(5);
        private List<int> noTouchPoints = new List<int>(5);
        private bool isPointedUI;
        public float hOffset = 0;
        public float vOffset = -20;
        public bool isCanDragOnUI = true;
        [Tooltip("是否右键可以拖动")]
        public bool isCanRightButtonDrag = true;
        [Tooltip("是否左键可以拖动")]
        public bool isCanLeftButtonDrag = true;

        public void ResetRotateLookAngle()
        {
	        m_PivotTargetRot = m_Pivot.localRotation;
	        m_PivotEulers = m_PivotTargetRot.eulerAngles;

	        m_TransformTargetRot = transform.localRotation; 
	        m_LookAngle = transform.localEulerAngles.y;
        }

        protected override void Awake()
        {
            base.Awake();
            m_Pivot = CameraTrans.parent;
            // Lock or unlock the cursor.
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;
			m_PivotEulers = m_Pivot.rotation.eulerAngles;

	        m_PivotTargetRot = m_Pivot.transform.localRotation;
			m_TransformTargetRot = transform.localRotation;
			
			Input.gyro.enabled = true;  
			
			Rotate(0, 0);
        }


        protected void Update()
        {
            HandleRotationMovement();
            if (m_LockCursor && Input.GetMouseButtonUp(0))
            {
                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LockCursor;
            }
        }


        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        protected override void FollowTarget(float deltaTime)
        {
            if (m_Target == null) return;
            // Move the rig towards target position.
//            transform.position = m_Target.position - transform.right * hOffset;
            transform.position = Vector3.Lerp(transform.position, m_Target.position - transform.right * hOffset, 0.5f);
//            transform.position = Vector3.Lerp(transform.position, m_Target.position - transform.right * hOffset, deltaTime*m_MoveSpeed);
        }
        
        


        private void HandleRotationMovement()
        {
			if(Time.timeScale < float.Epsilon)
			    return;

            var x = 0f;
            var y = 0f;
            if (Application.isMobilePlatform)
            {
	            if (Input.touches.Length > 0)
	            {
		            DeleteNoTouchPoint();
		            
		            foreach (var touch in Input.touches)
		            {
			            if (touch.phase == TouchPhase.Began)
			            {
				            fingerTouchDic[touch.fingerId] = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
			            }
			            if (! isCanDragOnUI && ! fingerTouchDic[touch.fingerId])
			            {
				            x += touch.deltaPosition.x * 0.04f;
				            y += touch.deltaPosition.y * 0.04f;
				            break;
			            }
		            }
	            }
            }
            else
            {
	            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
	            {
		            isPointedUI = EventSystem.current.IsPointerOverGameObject();
	            }
	            
	            if ((isCanLeftButtonDrag && Input.GetMouseButton(0)) || (Input.GetMouseButton(1) && isCanRightButtonDrag))
	            {
		            if (! isCanDragOnUI && isPointedUI)
		            {
			            x = y = 0;
		            }
		            else
		            {
			            x = Input.GetAxis("Mouse X");
			            y = Input.GetAxis("Mouse Y");
		            }
	            }
            }

            // 手机上发现是反的
            if (isGyroEnabled)
            {
	            x += -Input.gyro.rotationRate.y * 0.375f;
	            y += Input.gyro.rotationRate.x * 0.375f;
            }
            
            //
            if (! CanRotate)
            {
                x = y = 0;
            }

            if (Math.Abs(x) < 0.001f && Math.Abs(y) < 0.001f)
            {
	            return;
            }
            Rotate(x, y);
        }

        public void Rotate(float x, float y, SmoothingMode smoothingMode = SmoothingMode.TurnSmoothingCtrl, bool isPrint = false)
        {
	        
	        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
	        m_LookAngle += x*m_TurnSpeed;
			
	        // Rotate the rig (the root object) around Y axis only:
	        m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

	        if (m_VerticalAutoReturn)
	        {
		        // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
		        // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
		        // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
		        m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
	        }
	        else
	        {
		        // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
		        m_TiltAngle -= y*m_TurnSpeed;
		        // and make sure the new value is within the tilt range
		        m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
	        }

	        // Tilt input around X is applied to the pivot (the child of this object)
	        m_PivotTargetRot = Quaternion.Euler(m_TiltAngle + vOffset, m_PivotEulers.y , m_PivotEulers.z);

	        switch (smoothingMode)
	        {
		        case SmoothingMode.None:
			        m_Pivot.localRotation = m_PivotTargetRot;
			        transform.localRotation = m_TransformTargetRot;
			        break;
		        case SmoothingMode.Smoothing:
			        m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
			        transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
			        break;
		        case SmoothingMode.TurnSmoothingCtrl:
			        if (m_TurnSmoothing > 0)
			        {
				        m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
				        transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
			        }
			        else
			        {
				        m_Pivot.localRotation = m_PivotTargetRot;
				        transform.localRotation = m_TransformTargetRot;
			        }
			        break;
	        }
        }

        /// <summary>
        /// 删除没触摸的点
        /// </summary>
        public void DeleteNoTouchPoint()
        {
	        touchsTemp.Clear();
	        foreach (var touch in Input.touches)
	        {
		        touchsTemp.Add(touch.fingerId);
	        }
			noTouchPoints.Clear();
	        foreach (var pair in fingerTouchDic)
	        {
		        if (touchsTemp.Contains(pair.Key))
		        {
			        continue;
		        }
		        noTouchPoints.Add(pair.Key);
	        }

	        for (int i = 0; i < noTouchPoints.Count; i++)
	        {
		        fingerTouchDic.Remove(noTouchPoints[i]);
	        }
        }
    }
}
