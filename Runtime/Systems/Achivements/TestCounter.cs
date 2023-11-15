using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Sirenix.OdinInspector;

namespace Rufas.Achivements
{
    public class TestCounter : MonoBehaviour
    {
        [Button()] private void UpdateCounter()
        {
            Debug.Log("Update");
            Oculus.Platform.Achievements.AddCount("PLACE_TRACK_10", 1);
        }
    }
}
