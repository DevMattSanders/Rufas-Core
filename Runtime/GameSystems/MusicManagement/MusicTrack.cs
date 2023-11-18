using Rufas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.MusicManagement
{
    [CreateAssetMenu(menuName = "Rufas/Music/MusicTrack")]
    public class MusicTrack : ScriptableObject
    {
        public string trackName;
        [Space]
        public AudioClip audioClip;
    }
}
