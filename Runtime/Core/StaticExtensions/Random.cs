using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class Random
    {
        public static bool Bool()
        {
            float randomValue = UnityEngine.Random.value;
            if (randomValue >= 0.5f)
            {
                return true;
            }
            return false;
        }
    }
}
