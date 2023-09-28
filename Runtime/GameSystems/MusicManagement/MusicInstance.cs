using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.MusicManagement
{
    [RequireComponent(typeof(AudioSource))] public class MusicInstance : MonoBehaviour
    {
        [SerializeField, InlineEditor, ReadOnly] private MusicTrack myTrack;
        [Space]
        public FloatVariable musicVolume;
        [SerializeField, InlineEditor, ReadOnly] private AudioSource audioSource;

        public MusicInstance (FloatVariable volumeSO)
        {
            musicVolume = volumeSO;
        }

        private void Start()
        {
            musicVolume.AddListener(UpdateVolume);
        }

        private void OnDestroy()
        {
            musicVolume.RemoveListener(UpdateVolume);
        }


        public void UpdateVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void PlayNewMusicTrack(MusicTrack newTrack)
        {
            audioSource = GetComponent<AudioSource>();
            myTrack = newTrack;
            audioSource.clip = newTrack.audioClip;
            audioSource.Play();
        }

        public float GetTimeLeftOnCurrentTrack()
        {
            if (audioSource.isPlaying == false) { return 0f; }

            float timeLeft = audioSource.clip.length - audioSource.time;
            return timeLeft;
        }

        public MusicTrack GetMusicTrack()
        {
            return myTrack;
        }
    }
}
