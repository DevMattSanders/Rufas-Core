using Rufas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClockDisplay : MonoBehaviour
{
    [SerializeField] private IntVariable hour;
    [SerializeField] private IntVariable minute;
    [SerializeField] private FloatVariable second;
    [Space]
    [SerializeField] private RectTransform hourHand;
    [SerializeField] private RectTransform minuteHand;
    //[SerializeField] private RectTransform secondHand;

    void LateUpdate()
    {
        // Calculate the rotation angles for each hand
        float hoursAngle = 360f * (hour.Value % 12 + minute.Value / 60f) / 12f;
        float minutesAngle = 360f * (minute.Value + second.Value / 60f) / 60f;
        //float secondsAngle = 360f * (second.Value + Millisecond / 1000f) / 60f;

        // Apply the rotation to the hands' RectTransform components
        if (hourHand != null) { hourHand.rotation = Quaternion.Euler(0f, 0f, -hoursAngle); }
        if (minuteHand != null) { minuteHand.rotation = Quaternion.Euler(0f, 0f, -minutesAngle); }
        //if (secondHand != null) { secondHand.rotation = Quaternion.Euler(0f, 0f, -secondsAngle); }
    }
}
