using System;

using UnityEngine;

namespace Rufas
{
    public class RealWorldTime : MonoBehaviour
    {
        [SerializeField] private IntVariable hour;
        [SerializeField] private IntVariable minute;

        private void Update()
        {
            DateTime currentTime = DateTime.Now;

            if (hour == null || minute == null) { return; }

            hour.Value = currentTime.Hour;
            minute.Value = currentTime.Minute;
        }
    }
}
