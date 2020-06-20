using UnityEngine;

public enum SoundPlayEventType
{
    OnEnable,
    OnConllision,
}

public enum SoundType
{
    CommonSound,
    BackMusic,
}

[RequireComponent(typeof(AudioSource))]
public class SoundInstanceCtrl : MonoBehaviour
{
    public SoundType soundType = SoundType.CommonSound;
    public SoundPlayEventType EventType = SoundPlayEventType.OnEnable;
    public AudioClip clip;
    private AudioSource myAudioSource;
    [Tooltip("背景音乐不受此控制")]
    public bool loop = false;
    public bool is2D = true;

    // Start is called before the first frame update
    void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.playOnAwake = false;
//        myAudioSource.spatialBlend = 0.95f;
        myAudioSource.spatialBlend = 1f;
    }

    private void OnEnable()
    {
        if (DataManager.Instance.settingData == null)
        {
            Debug.LogError("DataManager 还未初始化!");
            return;
        }
        if (clip && EventType == SoundPlayEventType.OnEnable)
        {
            switch (soundType)
            {
                case SoundType.CommonSound:
                    if (DataManager.Instance.settingData.isOpenSound)
                    {
                        myAudioSource.clip = clip;
                        myAudioSource.loop = loop;
                        myAudioSource.mute = false;
                        myAudioSource.spatialBlend = is2D ? 0 : 1;
                        myAudioSource.Play();
                    }
                    break;
                case SoundType.BackMusic:
                    if (DataManager.Instance.settingData.isOpenSound)
                    {
                        SoundManager.Instance.StartPlayBgMusic(clip);
                    }
                    else
                    {
                        SoundManager.Instance.SetPlayBgMusic(clip);
                    }

                    break;
            }
        }
    }

    private void OnDisable()
    {
        if (clip)
        {
            myAudioSource.loop = false;
            myAudioSource.mute = true;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (DataManager.Instance.settingData.isOpenSound)
            switch (soundType)
            {
                case SoundType.CommonSound:
                    SoundManager.Instance.Play3DSound(clip, other.relativeVelocity.magnitude / 80, other.contacts[0].point);
                    break;
                case SoundType.BackMusic:
                    SoundManager.Instance.StartPlayBgMusic(clip);
                    break;
            }
    }
}
