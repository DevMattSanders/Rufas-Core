using Rufas;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.MusicManagement
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;

        [Header("Current & Upcoming")]
        [SerializeField, ReadOnly] public float timeLeftOnCurrentTrack;
        [SerializeField, InlineEditor, ReadOnly] public MusicTrack currentTrack;
        [Space]
        [SerializeField, InlineEditor, ReadOnly] public MusicTrack upcomingTrack;
        [Space]
        [Header("Track List")]
        [InlineEditor, DisableInPlayMode] public MusicTrackList currentTrackList;
        [Space]
        [Header("Volume Settings")]
        public FloatVariable musicVolume;
        [Space]
        [Header("Cross Fading")]
        [SerializeField] private bool isCurrentlyFading;
        [SerializeField] private float crossFadeDuration;
        [SerializeField] private float fadeTimer;
        [Space]
        [Header("Instances")]
        [FoldoutGroup("Instances"), SerializeField, InlineEditor, ReadOnly] private MusicInstance currentInstance;
        [FoldoutGroup("Instances"), SerializeField, InlineEditor, ReadOnly] private MusicInstance upcommingInstance;

        private void Awake()
        {
            if (Instance == null) { MusicManager.Instance = this; }
            else { Debug.LogError("Two or more instances of music manager!", this.gameObject); }
        }

        private void Update()
        {
            if (currentInstance != null)
            {
                timeLeftOnCurrentTrack = currentInstance.GetTimeLeftOnCurrentTrack();
                if (timeLeftOnCurrentTrack <= crossFadeDuration && !isCurrentlyFading)
                {
                    StartCrossFade();
                }
            }
            
            if (isCurrentlyFading)
            {
                if (fadeTimer < crossFadeDuration)
                {
                    float fadeProgress = (fadeTimer / crossFadeDuration) * musicVolume.Value;
                    currentInstance.UpdateVolume(musicVolume.Value - fadeProgress);

                    upcommingInstance.UpdateVolume(fadeProgress);

                    fadeTimer += Time.deltaTime;
                }
                else
                {
                    isCurrentlyFading = false;
                    fadeTimer = 0f;
                    Destroy(currentInstance.gameObject);
                    currentInstance = upcommingInstance;
                    upcommingInstance = null;
                    currentTrack = currentInstance.GetMusicTrack();
                }
            }
        }

        public void StartNewTrackList(MusicTrackList newTrackList)
        {
            crossFadeDuration = newTrackList.crossFadeDuration;
            currentTrackList = newTrackList;

            if (currentInstance != null)
            {
                StartCrossFade();
            }
            else
            {
                currentInstance = CreateNewMusicInstance();
                currentTrack = currentTrackList.GetNextUnplayedTrack();
                currentInstance.PlayNewMusicTrack(currentTrack);
            }
        }

        private MusicInstance CreateNewMusicInstance()
        {
            MusicInstance newInstance = new GameObject("Music Instance").AddComponent<MusicInstance>();
            
            newInstance.transform.SetParent(transform, false);
            newInstance.musicVolume = musicVolume;
            //newInstance.UpdateVolume(musicVolume.Value);

            return newInstance;
        }

        [Button()] private void StartCrossFade()
        {
            Debug.Log("Start Timer");
            isCurrentlyFading = true;
            fadeTimer = 0f;
            upcommingInstance = CreateNewMusicInstance();
            upcomingTrack = currentTrackList.GetNextUnplayedTrack();
            upcommingInstance.PlayNewMusicTrack(upcomingTrack);
        }
    }
}
