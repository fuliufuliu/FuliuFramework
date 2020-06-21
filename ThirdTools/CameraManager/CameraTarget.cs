using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraTarget : MonoBehaviour
{
    private Vector3 lastMousePosition;
    public Camera myCamera;
    private float dragScale = -0.002f;
    [Tooltip("是否右键可以拖动")]
    public bool isCanRightButtonDrag = true;
    [Tooltip("是否左键可以拖动")]
    public bool isCanMiddleButtonDrag = true;

    public bool isOnLateUpdate = true;

    private void Update()
    {
        if (!isOnLateUpdate) _Update();
    }

    private void LateUpdate()
    {
        if (isOnLateUpdate) _Update();
    }

    private void _Update()
    {
        if ( isCanMiddleButtonDrag && Input.GetMouseButton(2) ||  isCanRightButtonDrag && Input.GetMouseButton(1))
        {
            var deltaPos = Input.mousePosition - lastMousePosition;
            transform.position += (myCamera.transform.right * deltaPos.x + myCamera.transform.up * deltaPos.y)
                                  * Vector3.Distance(myCamera.transform.position, transform.position) * dragScale;
        }

        lastMousePosition = Input.mousePosition;
    }
}
