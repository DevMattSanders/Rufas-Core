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
    public struct BoolWithCallback : IEquatable<Boolean>
    {
        [SerializeField, HideInInspector]
        private bool _value;

        public BoolWithCallback(bool startingValue) : this()
        {
            Value = startingValue;
        }

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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(bool other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
               

        public static bool operator ==(BoolWithCallback a, bool b)
        {
            return a.Value == b;
        }
        public static bool operator !=(BoolWithCallback a, bool b)
        {
            return !(a.Value == b);
        }
       
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
