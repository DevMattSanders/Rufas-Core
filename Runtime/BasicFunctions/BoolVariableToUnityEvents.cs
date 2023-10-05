using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas.BasicFunctions
{
    public class BoolVariableToUnityEvents : MonoBehaviour
    {
        public BoolVariable boolVariable;

        public UnityEvent<bool> OnValueChanged;
        public UnityEvent onTrue;
        public UnityEvent onFalse;

        private void Start()
        {
            boolVariable.AddListener(ValueChanged,true);
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
