using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas.BasicFunctions
{
    public class UnityEventOnDestroy : MonoBehaviour
    {
        public UnityEvent unityEvent = new UnityEvent();

        private void OnDestroy()
        {
            unityEvent.Invoke();
        }
    }
}
