using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class LocalBoolVariableToUnityEvents : MonoBehaviour
    {
        public LocalBoolVariable boolVariable;

        public UnityEvent<bool> OnValueChanged;
        public UnityEvent onTrue;
        public UnityEvent onFalse;

        public bool refreshOnStart;

        private void Start()
        {
            boolVariable.AddListener(ValueChanged, true);

            if (refreshOnStart)
            {
                ValueChanged(boolVariable.Value);
            }
        }

        private void OnDestroy()
        {
            boolVariable.RemoveListener(ValueChanged);
        }

        private void ValueChanged(bool val)
        {
            OnValueChanged.Invoke(val);

            if (val)
            {
                onTrue.Invoke();
            }
            else
            {
                onFalse.Invoke();
            }
        }
    }
}
