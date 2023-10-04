using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas.BasicFunctions
{
    public class VoidEventToUnityEvent : MonoBehaviour
    {
        public VoidEvent voidEvent;
        public UnityEvent unityEvent;


        private void Start()
        {
            voidEvent.AddListener(unityEvent.Invoke);
        }

        private void OnDestroy()
        {
            voidEvent.RemoveListener(unityEvent.Invoke);
        }
    }
}
