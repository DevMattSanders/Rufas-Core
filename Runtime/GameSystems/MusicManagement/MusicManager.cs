using Rufas;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.MusicManagement
{
    public class MusicManager : GameSystem<MusicManager>
    {
        //public static MusicManager Instance;

        public Transform musicInstancesParent;

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
        [FoldoutGroup("Instances"), SerializeField, InlineEditor, ReadOnly] private MusicInstance upcomingInstance;

        // private void Awake()
        // {
        //    if (Instance == null) { MusicManager.Instance = this; }
        //     else { Debug.LogError("Two or more instances of music manager!", this.gameObject); }
        // }

        public override string DesiredName()
        {
            return "Music Manager";

        }

        private void ResetValues()
        {
            timeLeftOnCurrentTrack = 0;
            currentTrack = null;
            upcomingTrack = null;
            currentInstance = null;
            isCurrentlyFading = false;
            crossFadeDuration = 0;
            fadeTimer = 0;
            currentInstance = null;
            upcomingInstance = null;
        }

        public override void OnAwakeBehaviour()
        {
            base.OnAwakeBehaviour();
            ResetValues();
        }

        public override void EndOfApplicaitonBehaviour()
        {
            base.EndOfApplicaitonBehaviour();
            ResetValues();
        }

        public override void OnStartBehaviour()
        {
            base.OnStartBehaviour();

            CoroutineMonoBehaviour.i.StartCoroutine(MusicManagerUpdate());

            musicInstancesParent = new GameObject("Music-Instances").transform;
            DontDestroyOnLoad(musicInstancesParent);

        }

        IEnumerator MusicManagerUpdate()
        {
            while (true)
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

                        upcomingInstance.UpdateVolume(fadeProgress);

                        fadeTimer += Time.deltaTime;
                    }
                    else
                    {
                        isCurrentlyFading = false;
                        fadeTimer = 0f;
                        if (currentInstance != null) { Destroy(currentInstance.gameObject); }
                        currentInstance = upcomingInstance;
                        upcomingInstance = null;
                        if (currentInstance != null)
                        {
                            currentTrack = currentInstance.GetMusicTrack();
                        }
                    }
                }
                yield return null;
            }
        }

        public void StartNewTrackList(MusicTrackList newTrackList)
        {
            if (isCurrentlyFading || newTrackList == currentTrackList) { return; }

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

            DontDestroyOnLoad(newInstance.gameObject);
            newInstance.transform.SetParent(musicInstancesParent, false);
            newInstance.musicVolume = musicVolume;

            return newInstance;
        }

        [Button()] private void StartCrossFade()
        {
            isCurrentlyFading = true;
            fadeTimer = 0f;
            upcomingInstance = CreateNewMusicInstance();
            upcomingTrack = currentTrackList.GetNextUnplayedTrack();
            upcomingInstance.PlayNewMusicTrack(upcomingTrack);
        }
    }
}
