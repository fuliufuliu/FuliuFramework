using System.Threading.Tasks;
using UnityEngine;

public class SoundManager : SingleBhv<SoundManager>
{
    public AudioSource bgMusicSource;
    public Transform d2SoundSourceParent;
    public AudioSource d2SoundSourceTemp;
    string d2SoundSourceTempKey = "d2SoundSourceTemp";
    string d3SoundSourceTempKey = "d3SoundSourceTemp";
    public Transform d3SoundSourceParent;
    public AudioSource d3SoundSourceTemp;


    public override void Initial()
    {
        base.Initial();
        if (Instance && Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        GoPoolManager.Instance.CreatePool(d2SoundSourceTempKey, d2SoundSourceTemp.gameObject);
        GoPoolManager.Instance.CreatePool(d3SoundSourceTempKey, d3SoundSourceTemp.gameObject);
    }
//
//    public override void Init()
//    {
//        base.Init();
//    }

    public void SetPlayBgMusic(AudioClip clip)
    {
        if (clip)
        {
            bgMusicSource.clip = clip;
        }
    }
    
    public void StartPlayBgMusic(AudioClip clip = null)
    {
        if (clip)
        {
            bgMusicSource.clip = clip;
        }

        bgMusicSource.enabled = true;
        bgMusicSource.Stop();
        bgMusicSource.Play();
    }

    public void StopPlayBgMusic()
    {
        bgMusicSource.enabled = false;
    }

    AudioClip LoadSound(string resourcesPath)
    {
        return LoadManager.Load<AudioClip>(resourcesPath);
    }
    public void Play2DSound(string resourcesPath)
    {
        //        Start2DSound(LoadSound(resourcesPath));
        _Play2DSound(resourcesPath);
    }
    async Task _Play2DSound(string resourcesPath)
    {
        var clip = await LoadManager._LoadAsync<AudioClip>(resourcesPath);
        Start2DSound(clip);
    }
    
    void Start2DSound(AudioClip clip)
    {
        if (clip)
        {
            var go = GoPoolManager.Instance.Pop(d2SoundSourceTempKey);
            var audio = go.GetComponent<AudioSource>();
            audio.transform.SetParent(d2SoundSourceParent);
            audio.transform.Reset();
            audio.clip = clip;
            audio.volume = 0.8f;
            audio.enabled = true;
            audio.Stop();
            audio.Play();
            CoroutineHelper.Instance.Delay(clip.length, () =>
            {
                GoPoolManager.Instance.Push(d2SoundSourceTempKey, go);
            });
        }
    }
    
    void Start3DSound(AudioClip clip, float volume, Vector3 worldPos)
    {
        if (clip)
        {
            var go = GoPoolManager.Instance.Pop(d3SoundSourceTempKey);
            var audio = go.GetComponent<AudioSource>();
            audio.transform.SetParent(d3SoundSourceParent);
            audio.transform.Reset();
            audio.transform.position = worldPos;
            audio.clip = clip;
            audio.volume = volume;
            audio.enabled = true;
            audio.Stop();
            audio.Play();
            CoroutineHelper.Instance.Delay(clip.length, () =>
            {
                GoPoolManager.Instance.Push(d3SoundSourceTempKey, go);
            });
        }
    }

    public void Play3DSound(string resourcesPath, float volume, Vector3 worldPos)
    {
        _Play3DSound(resourcesPath, volume, worldPos);
    }
    
    public void Play3DSound(AudioClip clip, float volume, Vector3 worldPos)
    {
        Start3DSound(clip, volume, worldPos);
    }
    
    async Task _Play3DSound(string resourcesPath, float volume, Vector3 worldPos)
    {
        var clip = await LoadManager._LoadAsync<AudioClip>(resourcesPath);
        Start3DSound(clip, volume, worldPos);
    }

}
