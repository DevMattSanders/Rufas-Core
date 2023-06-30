using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class RufasDebugger
    {
        private static bool debugMessages = false;
        public static void Log(string message)
        {
            if(debugMessages) Debug.Log("{ RUFAS } - " +  message);
        }

        public static void Warning(string warning)
        {
            if (debugMessages) Debug.LogWarning("{ RUFAS } - " + warning);
        }

        public static void Error(string error)
        {
            if (debugMessages) Debug.LogError("{ RUFAS } - " + error);
        }
    }
}
