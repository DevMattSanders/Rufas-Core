using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rufas
{
    public class DigitialClockDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text displayText;

        [SerializeField] private IntVariable hour;
        [SerializeField] private IntVariable minute;

        private void LateUpdate()
        {
            if (displayText != null)
            {
                string hourString = hour.Value.ToString();
                if (hour.Value <= 9) { hourString = "0" + hour.Value.ToString("F0"); }

                string minuteString = minute.Value.ToString();
                if (minute.Value <= 9) { minuteString = "0" + minute.Value.ToString("F0"); }

                displayText.SetText(hourString + " : " + minuteString);
            }
        }
    }
}
