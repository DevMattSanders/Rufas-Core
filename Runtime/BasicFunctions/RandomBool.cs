using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.BasicFunctions
{
    public static class RandomBool
    {
        public static bool GetBool()
        {
            float randomValue = Random.value;
            if (randomValue >= 0.5f)
            {
                return true;
            }
            return false;
        }
    }
}
