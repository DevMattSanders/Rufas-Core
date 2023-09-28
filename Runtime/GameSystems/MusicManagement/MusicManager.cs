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

        [SerializeField, InlineEditor, ReadOnly] public MusicTrack currentTrack;
        [SerializeField, ReadOnly] public float timeLeftOnCurrentTrack;
        [SerializeField, InlineEditor, ReadOnly] public MusicTrack upcomingTrack;
        [Space]
        [InlineEditor, ReadOnly] public MusicTrackList currentTrackList;
        [Space]
        [FoldoutGroup("Settings")] public FloatVariable musicVolume;
        [FoldoutGroup("Settings")] public float fadeDurationTime;
        [Space]
        [FoldoutGroup("Music Instances"), SerializeField] private bool isCurrentlyFading;
        [FoldoutGroup("Music Instances"), SerializeField] private float fadeTimer;
        [FoldoutGroup("Music Instances"), SerializeField, InlineEditor, ReadOnly] private MusicInstance currentInstance;
        [FoldoutGroup("Music Instances"), SerializeField, InlineEditor, ReadOnly] private MusicInstance upcommingInstance;

        private void Awake()
        {
            if (Instance == null) { MusicManager.Instance = this; }
            else { Debug.LogError("Two or more instances of music manager!", this.gameObject); }
        }

        private void Start()
        {
            currentInstance = CreateNewMusicInstance();
            currentTrack = currentTrackList.GetNextUnplayedTrack();
            currentInstance.PlayNewMusicTrack(currentTrack);
        }

        private void Update()
        {
            timeLeftOnCurrentTrack = currentInstance.GetTimeLeftOnCurrentTrack();
            if (timeLeftOnCurrentTrack <= fadeDurationTime && !isCurrentlyFading)
            {
                StartCrossFadeToNextTrack();
            }

            if (isCurrentlyFading)
            {
                if (fadeTimer < fadeDurationTime)
                {
                    float fadeProgress = (fadeTimer / fadeDurationTime) * musicVolume.Value;
                    currentInstance.UpdateMusicInstance(musicVolume.Value - fadeProgress);

                    upcommingInstance.UpdateMusicInstance(fadeProgress);

                    fadeTimer += Time.deltaTime;
                }
                else
                {
                    isCurrentlyFading = false;
                    fadeTimer = 0f;
                    Destroy(currentInstance.gameObject);
                    currentInstance = upcommingInstance;
                    currentTrack = currentInstance.GetMusicTrack();
                }
            }
        }

        private MusicInstance CreateNewMusicInstance()
        {
            MusicInstance newInstance = new GameObject("Music Instance").AddComponent<MusicInstance>();
            newInstance.transform.SetParent(transform, false);
            return newInstance;
        }

        [Button()] private void StartCrossFadeToNextTrack()
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
