using UnityEngine;

public class DeletePref : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs.DeleteAll()");
    }
}
