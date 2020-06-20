using UnityEngine;

public class Disabled : MonoBehaviour
{
    void Awake()
    {
        DestroyImmediate(gameObject);
    }
}
