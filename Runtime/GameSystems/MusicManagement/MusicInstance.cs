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
        [SerializeField, InlineEditor, ReadOnly] private AudioSource audioSource;

        public void InitMusicInstance(float volume)
        {
            audioSource = GetComponent<AudioSource>();
            UpdateVolume(volume);
        }

        public void UpdateVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void PlayNewMusicTrack(MusicTrack newTrack)
        {
            InitMusicInstance(MusicManager.Instance.musicVolume.Value);
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
