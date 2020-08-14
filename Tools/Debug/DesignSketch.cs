using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 效果图控制
/// </summary>
[ExecuteInEditMode]
public class DesignSketch : MonoBehaviour
{
    private Thread myThread;
    private System.Random myRandom;
    private Timer timer;
    private RawImage myRawImage;

    private void Update()
    { 
        if (myRawImage == null) 
            myRawImage = GetComponent<RawImage>();
        if (myRawImage != null) myRawImage.color = new Color(1, 1, 1, Random.Range(0,1f));
    }
}
