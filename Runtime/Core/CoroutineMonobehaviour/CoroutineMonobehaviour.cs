using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class CoroutineMonobehaviour : MonoBehaviour
    {
        private static CoroutineMonobehaviour generatedInstance;
        public static MonoBehaviour i
        {
            get
            {
                if (!generatedInstance) generatedInstance = new GameObject("CoroutineMonobehaviour").AddComponent<CoroutineMonobehaviour>();

                return generatedInstance;
            }
        }
    }
}