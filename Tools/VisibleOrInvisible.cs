using System;
using UnityEngine;

public class VisibleOrInvisible : MonoBehaviour
{
    [NonSerialized]public bool isVisible;

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}