using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class VariablesWithCallbacks
    {
       
    }

    [System.Serializable, InlineProperty]
    public struct BoolWithCallback
    {
        private bool _value;
        [ShowInInspector, HideLabel]
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

        public event Action<bool> onValue;       
    }
    [System.Serializable, InlineProperty]
    public struct FloatWithCallback
    {
        private float _value;
        [ShowInInspector, HideLabel]
        public float Value
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

        public event Action<float> onValue;
    }
}
