using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas.BasicFunctions
{
    public class UnityEventOnStartAfterDelay : MonoBehaviour
    {
        [SerializeField] private float delay;
        [SerializeField] private UnityEvent unityEvent = new UnityEvent();

        private void Start()
        {
            this.CallWithDelay(CallEvent, delay);
        }

        private void CallEvent()
        {
            unityEvent.Invoke();
        }
    }
}
