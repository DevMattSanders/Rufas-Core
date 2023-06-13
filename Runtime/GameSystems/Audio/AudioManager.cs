using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Global Volumes")]
        [Required] public FloatVariable masterVolume;
        [Required] public FloatVariable gameVolume;
        [Required] public FloatVariable musicVolume;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(Instance); }
        }

        public float CalcuateVolume(float localVolume)
        {
            float volume = localVolume;
            volume = volume * gameVolume.Value; //*= masterVolume.Value;

            return volume;
        }
    }
}
