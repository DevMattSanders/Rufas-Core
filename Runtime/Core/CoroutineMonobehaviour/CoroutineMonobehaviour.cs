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
                if (!generatedInstance)
                {
                    generatedInstance = new GameObject("CoroutineMonobehaviour").AddComponent<CoroutineMonoBehaviour>();
                    GameObject.DontDestroyOnLoad(generatedInstance.gameObject);
                }

                return generatedInstance;
            }
        }

        public static void StartCoroutine(IEnumerator Routine, IEnumerator routineInstance = null)
        {
            if (routineInstance == null)
            {
                i.StartCoroutine(Routine);
            }
            else
            {
                if (routineInstance != null)
                {
                    i.StopCoroutine(routineInstance);
                }

                routineInstance = Routine;

                i.StartCoroutine(routineInstance);
            }
        } 
    }
}