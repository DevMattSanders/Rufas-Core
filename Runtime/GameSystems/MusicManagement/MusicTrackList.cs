using Rufas;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Rufas.MusicManagement
{
    public class MusicTrackList : SuperScriptable
    {
        [Header("Settings")]
        public float crossFadeDuration;

        [Header("Tracks")]
        [DisableInPlayMode, SerializeField] private List<MusicTrack> musicTracks = new List<MusicTrack>();
        [DisableInEditorMode, SerializeField] private List<MusicTrack> unplayedTracks;

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            CreateUnplayedList();
        }

        public MusicTrack GetNextUnplayedTrack()
        {
            if (unplayedTracks.Count == 0) { 
                CreateUnplayedList(); 
            }

            MusicTrack nextTrack = unplayedTracks[Random.Range(0, unplayedTracks.Count)];
            unplayedTracks.Remove(nextTrack);
            return nextTrack;
        }

        private void CreateUnplayedList()
        {
            unplayedTracks = new List<MusicTrack>();
            foreach (MusicTrack track in musicTracks)
            {
                unplayedTracks.Add(track);
            }
        }
    }
}
