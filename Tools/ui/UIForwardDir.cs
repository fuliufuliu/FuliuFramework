using UnityEngine;

/// <summary>
/// 让UI保持一个反向，不受parent的反向影响。
/// </summary>
public class UIForwardDir : MonoBehaviour
{
    public Vector3 angle;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = angle;
    }
}
