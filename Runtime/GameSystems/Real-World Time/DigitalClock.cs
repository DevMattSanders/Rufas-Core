using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rufas
{
    public class DigitalClock : MonoBehaviour
    {
        [SerializeField] private TMP_Text clockText;
        [Space]
        [SerializeField] private IntVariable hour;
        [SerializeField] private IntVariable minute;

        private void Update()
        {
            if (hour == null || minute == null)
            { 
                clockText.SetText("??:??"); 
            }
            else
            {
                string hourText = hour.Value.ToString();
                string minText = minute.Value.ToString();

                if (hour.Value <= 9)
                {
                    hourText = "0" + hour.Value.ToString();
                }

                if (minute.Value <= 9)
                {
                    minText = "0" + minute.Value.ToString();
                }

                clockText.SetText(hourText + ":" + minText);
            }
        }
    }
}
