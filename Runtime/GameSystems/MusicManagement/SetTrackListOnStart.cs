using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.MusicManagement
{
    public class SetTrackListOnStart : RufasMonoBehaviour
    {
        [SerializeField] private MusicTrackList trackList;

        public override void Start_AfterInitialisation()
        {
            base.Start_AfterInitialisation();
        
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.StartNewTrackList(trackList);
            }
            else
            {
                Debug.LogError("Could not find music manager singleton", this.gameObject);
            }
        }
    }
}
