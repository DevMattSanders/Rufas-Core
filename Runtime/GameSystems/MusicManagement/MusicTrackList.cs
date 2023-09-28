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
        public bool playInRandomOrder;
        private int nonRandomIndex;
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
            MusicTrack nextTrack;

            if (unplayedTracks.Count == 0) { 
                CreateUnplayedList(); 
            }

            if (playInRandomOrder)
            {
                nextTrack = unplayedTracks[Random.Range(0, unplayedTracks.Count)];
                unplayedTracks.Remove(nextTrack);
                return nextTrack;
            }
            else
            {
                nextTrack = unplayedTracks[nonRandomIndex];
                nonRandomIndex++;
                if (nonRandomIndex >= unplayedTracks.Count) { nonRandomIndex = 0; }
            }
            
            return nextTrack;
        }

        private void CreateUnplayedList()
        {
            unplayedTracks = new List<MusicTrack>();
            foreach (MusicTrack track in musicTracks)
            {
                unplayedTracks.Add(track);
            }

            nonRandomIndex = 0;
        }
    }
}
