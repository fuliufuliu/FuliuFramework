using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraManager : SingleBhv<CameraManager> {
    public FreeLookCam freeLookCam; 
    public Camera myCamera;

    public Transform target;

    // Use this for initialization
//    void Awake ()
//	{
//	    freeLookCam = GetComponentInChildren<FreeLookCam>();
//        myCamera = GetComponentInChildren<Camera>();
//	}

}
