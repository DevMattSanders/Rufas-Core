using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class LocalStringVariableToUnityEvent : MonoBehaviour
    {
        public LocalStringVariable stringVariable;

        public UnityEvent<string> OnValueChanged;

        public bool refreshOnStart;
        
        private void Start()
        {
            stringVariable.AddListener(ValueChanged, true);

            if (refreshOnStart)
            {
                ValueChanged(stringVariable.Value);
            }
        }

        private void OnDestroy()
        {
            stringVariable.RemoveListener(ValueChanged);
        }

        private void ValueChanged(string val)
        {
            OnValueChanged.Invoke(val);
        }
    }
}
