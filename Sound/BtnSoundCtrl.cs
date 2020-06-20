using UnityEngine;

namespace _Game.Scripts.Framework.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class BtnSoundCtrl : MonoBehaviour
    {
        public AudioClip clip;
        private AudioSource myAudioSource;


        void Awake()
        {
            myAudioSource = GetComponent<AudioSource>();
            myAudioSource.playOnAwake = false;
//        myAudioSource.spatialBlend = 0.95f;
            myAudioSource.spatialBlend = 1f;
        }

        public void OnPlaySound()
        {
            if (DataManager.Instance.settingData.isOpenSound)
            {
                myAudioSource.clip = clip;
                myAudioSource.loop = false;
                myAudioSource.mute = false;
                myAudioSource.spatialBlend = 0;
                myAudioSource.Play();
            }
        }
    }
}