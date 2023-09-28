using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.MusicManagement
{
    public class SetTrackListOnStart : MonoBehaviour
    {
        [SerializeField] private MusicTrackList trackList;

        private void Start()
        {
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
