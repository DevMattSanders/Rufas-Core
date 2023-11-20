using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class GameSessionInstance : MonoBehaviour
    {
        public static List<GameSessionInstance> gameSessions = new List<GameSessionInstance>();

        private void Awake()
        {
            gameSessions.Add(this);
        }

        private void OnDestroy()
        {
            gameSessions.Remove(this);
        }


    }
}
