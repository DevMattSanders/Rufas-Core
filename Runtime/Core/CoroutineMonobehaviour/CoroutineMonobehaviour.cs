using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class CoroutineMonoBehaviour : MonoBehaviour
    {
        private static CoroutineMonoBehaviour generatedInstance;
        public static MonoBehaviour i
        {
            get
            {
                if (!generatedInstance) generatedInstance = new GameObject("CoroutineMonobehaviour").AddComponent<CoroutineMonoBehaviour>();

                return generatedInstance;
            }
        }
    }
}