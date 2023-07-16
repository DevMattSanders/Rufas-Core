using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class FloatExtensions
    {
        public static bool CompareWithTolerance(this float thisValue, float toCompare, float tolerance)
        {
            return Mathf.Abs(thisValue - toCompare) <= tolerance;
        }

        public static bool CompareWithTolerance(this FloatWithCallback thisValue, float toCompare, float tolerance)
        {
            return CompareWithTolerance(thisValue.Value, toCompare,tolerance);
        }

        public static bool CompareWithTolerance(this float thisValue, FloatWithCallback toCompare, float tolerance)
        {
            return CompareWithTolerance(thisValue,toCompare.Value,tolerance);
        }
    }
}
