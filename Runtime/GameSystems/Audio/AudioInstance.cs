using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioInstance : MonoBehaviour
    {
        [SerializeField, ReadOnly] private AudioSource audioSource;

        [SerializeField, Range(0, 1)] private float localVolume = 1f;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        [Button()]
        public void PlayAudioUsingManager(AudioClip audioClip, bool shouldLoop)
        {
            audioSource.Stop();
            float soundVolume = AudioManager.Instance.CalcuateVolume(localVolume);
            Debug.Log(soundVolume);
            audioSource.loop = shouldLoop;
            audioSource.volume = soundVolume;
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void StopAudio()
        {
            audioSource.Stop();
        }
    }
}
