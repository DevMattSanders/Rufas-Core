using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [InlineEditor, InlineProperty]
    public class LocalBoolVariable : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private bool _value;

        [ShowInInspector, HideLabel,InlineProperty]
        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == _value)
                {
                    return;
                }

                _value = value;
                onValue?.Invoke(_value);
            }
        }

        public void AddListener(System.Action<bool> listener, bool invokeNow = false)
        {
            onValue += listener;

            if (invokeNow) listener.Invoke(_value);
        }


        public void RemoveListener(System.Action<bool> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<bool> onValue;
        
    }
}
