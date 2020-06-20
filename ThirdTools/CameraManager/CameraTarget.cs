using System;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Vector3 lastMousePosition;
    public Camera myCamera;
    private float dragScale = -0.002f;

    private void LateUpdate()
    {
        if (Input.GetMouseButton(2))
        {
            var deltaPos = Input.mousePosition - lastMousePosition;
            transform.position += (myCamera.transform.right * deltaPos.x + myCamera.transform.up * deltaPos.y)
                                  * Vector3.Distance(myCamera.transform.position, transform.position) * dragScale;
        }

        lastMousePosition = Input.mousePosition;
    }
}
