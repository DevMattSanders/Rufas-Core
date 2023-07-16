using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class ColorExtensions
    {
        public static bool CompareWithTolerance(this Color thisColor, Color toCompare, float tolerancePercent)
        {
#if UNITY_EDITOR
            if (tolerancePercent > 1)
            {
                Debug.Log("Tolerance Percent over 1!. Range is 0-1");
            }
            else if (tolerancePercent < 0)
            {
                Debug.Log("Tolerance Percent uner 0!. Range is 0-1");
            }
#endif
            return Mathf.Abs(thisColor.r - toCompare.r) <= tolerancePercent &&
                   Mathf.Abs(thisColor.g - toCompare.g) <= tolerancePercent &&
                   Mathf.Abs(thisColor.b - toCompare.b) <= tolerancePercent &&
                   Mathf.Abs(thisColor.a - toCompare.a) <= tolerancePercent;
        }

        public static bool CompareWithTolerance(this ColorWithCallback thisColor, Color toCompare, float tolerancePercent)
        {
            return CompareWithTolerance(thisColor.Value, toCompare, tolerancePercent);
        }

        public static bool CompareWithTolerance(this Color thisColor, ColorWithCallback toCompare, float tolerancePercent)
        {
            return CompareWithTolerance(thisColor, toCompare.Value, tolerancePercent);
        }
    }
}
