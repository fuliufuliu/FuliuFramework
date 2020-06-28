using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraManager : SingleBhv<CameraManager> {
    public FreeLookCam freeLookCam; 
    public ProtectCameraFromWallClip cameraDistanceCtrl; 
    public Camera myCamera;

    public Transform target;

    public void SetDistance(float distance)
    {
        print($"distance: {distance}");
        if (distance > 0)
        {
            cameraDistanceCtrl.closestDistance = distance * 4;
        }
    }
}
