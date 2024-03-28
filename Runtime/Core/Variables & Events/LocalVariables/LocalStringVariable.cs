using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [InlineEditor, InlineProperty]
    public class LocalStringVariable : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private string _value;

        [ShowInInspector, HideLabel, InlineProperty]
        public string Value
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

        public void AddListener(System.Action<string> listener, bool invokeNow = false)
        {
            onValue += listener;

            if (invokeNow) listener.Invoke(_value);
        }


        public void RemoveListener(System.Action<string> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<string> onValue;
    }
}
