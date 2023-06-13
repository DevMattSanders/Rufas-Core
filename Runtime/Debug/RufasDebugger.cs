using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class RufasDebugger
    {
        public static void Log(string message)
        {
            Debug.Log("{ RUFAS } - " +  message);
        }

        public static void Warning(string warning)
        {
            Debug.LogWarning("{ RUFAS } - " + warning);
        }

        public static void Error(string error)
        {
            Debug.LogError("{ RUFAS } - " + error);
        }
    }
}
