using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 效果图控制
/// </summary>
[ExecuteInEditMode]
public class DesignSketch : MonoBehaviour
{
    private Thread myThread;
    private System.Random myRandom;
    public bool isRun = true;
    private Timer timer;

    private void OnValidate()
    {
        var myRawImage = GetComponent<RawImage>();

        if (myRawImage)
        {
            if (myRandom == null)
            {
                myRandom = new System.Random();
            }

            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
            
            timer = new Timer((a) =>
            {
                if (isRun)
                {
                    myRawImage.color = new Color(1,1,1, myRandom.Next(0, 100)/100f);
                    myRawImage.enabled = !myRawImage.enabled;
                }

            }, null, 500, 500);
        }
    }
}
