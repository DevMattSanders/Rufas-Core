using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
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

        public void AddListener(System.Action<bool> listener, bool callNow = false)
        {
            onValue += listener;

            if (callNow)
            {
                listener(Value);
            }
        }

        public void RemoveListener(System.Action<bool> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<bool> onValue;

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

        public static bool operator ==(bool a, BoolWithCallback b)
        {
            return a == b.Value;
        }
        public static bool operator !=(bool a, BoolWithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(BoolWithCallback a, BoolWithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator!=(BoolWithCallback a, BoolWithCallback b)
        {
            return !a.Value == b.Value;
        }
       
    }

    [System.Serializable, InlineProperty]
    public struct ColorWithCallback : IEquatable<Color>
    {
        [SerializeField, HideInInspector]
        private Color _value;         

        public ColorWithCallback(Color startingValue) : this()
        {
            Value = startingValue;
        }

        [ShowInInspector, HideLabel,HorizontalGroup("H")]
        public Color Value
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

        public void AddListener(System.Action<Color> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<Color> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<Color> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Color other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ColorWithCallback a, Color b)
        {
            return a.Value == b;
        }
        public static bool operator !=(ColorWithCallback a, Color b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(Color a, ColorWithCallback b)
        {
            return a == b.Value;
        }

        public static bool operator !=(Color a, ColorWithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(ColorWithCallback a, ColorWithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(ColorWithCallback a, ColorWithCallback b)
        {
            if (a.Value == b.Value) return false; return true;
        }      
    }

    [System.Serializable, InlineProperty]
    public struct DoubleWithCallback : IEquatable<double>
    {
        [SerializeField, HideInInspector]
        private double _value;

        public DoubleWithCallback(double startingValue) : this()
        {
            Value = startingValue;
        }

        [ShowInInspector, HideLabel, HorizontalGroup("H")]
        public double Value
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

        public void AddListener(System.Action<double> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<double> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<double> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(double other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(DoubleWithCallback a, double b)
        {
            return a.Value == b;
        }
        public static bool operator !=(DoubleWithCallback a, double b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(double a, DoubleWithCallback b)
        {
            return a == b.Value;
        }

        public static bool operator !=(double a, DoubleWithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(DoubleWithCallback a, DoubleWithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(DoubleWithCallback a, DoubleWithCallback b)
        {
            if (a.Value == b.Value) return false; return true;
        }
    }

    [System.Serializable, InlineProperty]
    public struct FloatWithCallback : IEquatable<float>
    {
        [SerializeField, HideInInspector]
        private float _value;

        [SerializeField, HideInInspector]
        private bool useTolerance;

        [SerializeField, HideInInspector]
        private float tolerance;

        public FloatWithCallback(float startingValue) : this()
        {
            Value = startingValue;
        }

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
              //  Debug.Log("Set value: " + Value + " " + value + " " + _value);
                onValue?.Invoke(_value);
              //  Debug.Log("InvokedEvent value: " + Value + " " + value + " " + _value);
            }
        }

        public void AddListener(System.Action<float> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<float> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<float> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(float other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static bool operator ==(FloatWithCallback a, float b)
        {
            return a.Value == b;
        }
        public static bool operator !=(FloatWithCallback a, float b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(float a, FloatWithCallback b)
        {
            return a == b.Value;
        }
        public static bool operator !=(float a, FloatWithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(FloatWithCallback a, FloatWithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(FloatWithCallback a, FloatWithCallback b)
        {
            if (a.Value == b.Value) return false;

            return true;
        }

    }

    [System.Serializable, InlineProperty]
    public struct GameObjectWithCallback : IEquatable<GameObject>
    {
        [SerializeField, HideInInspector]
        private GameObject _value;

        public GameObjectWithCallback(GameObject startingValue) : this()
        {
            Value = startingValue;
        }

        [ShowInInspector, HideLabel]
        public GameObject Value
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

        public void AddListener(System.Action<GameObject> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<GameObject> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<GameObject> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(GameObject other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(GameObjectWithCallback a, GameObject b)
        {
            return a.Value == b;
        }
        public static bool operator !=(GameObjectWithCallback a, GameObject b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(GameObject a, GameObjectWithCallback b)
        {
            return a == b.Value;
        }
        public static bool operator !=(GameObject a, GameObjectWithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(GameObjectWithCallback a, GameObjectWithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(GameObjectWithCallback a, GameObjectWithCallback b)
        {
            if (a.Value == b.Value) return false;

            return true;
        }
    }

    [System.Serializable, InlineProperty]
    public struct IntWithCallback : IEquatable<int>
    {
        [SerializeField, HideInInspector]
        private int _value;

        public IntWithCallback(int startingValue) : this()
        {
            Value = startingValue;
        }

        [ShowInInspector, HideLabel]
        public int Value
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

        public void AddListener(System.Action<int> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<int> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<int> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(int other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(IntWithCallback a, int b)
        {
            return a.Value == b;
        }
        public static bool operator !=(IntWithCallback a, int b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(int a, IntWithCallback b)
        {
            return a == b.Value;
        }
        public static bool operator !=(int a, IntWithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(IntWithCallback a, IntWithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(IntWithCallback a, IntWithCallback b)
        {
            if (a.Value == b.Value) return false;

            return true;
        }
    }
       
    [System.Serializable, InlineProperty]
    public struct StringWithCallback : IEquatable<string>
    {
        [SerializeField, HideInInspector]
        private string _value;

        public StringWithCallback(string startingValue) : this()
        {
            Value = startingValue;
        }

        [ShowInInspector, HideLabel]
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

        public void AddListener(System.Action<string> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<string> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<string> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(string other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static bool operator ==(StringWithCallback a, string b)
        {
            return a.Value == b;
        }
        public static bool operator !=(StringWithCallback a, string b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(string a, StringWithCallback b)
        {
            return a == b.Value;
        }
        public static bool operator !=(string a, StringWithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(StringWithCallback a, StringWithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(StringWithCallback a, StringWithCallback b)
        {
            if (a.Value == b.Value) return false;

            return true;
        }

    }

    [System.Serializable, InlineProperty]
    public struct Vector2WithCallback : IEquatable<Vector2>
    {
        [SerializeField, HideInInspector]
        private Vector2 _value;

        public Vector2WithCallback(Vector2 startingValue) : this()
        {
            Value = startingValue;
        }

        [ShowInInspector, HideLabel]
        public Vector2 Value
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

        public void AddListener(System.Action<Vector2> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<Vector2> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<Vector2> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Vector2 other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vector2WithCallback a, Vector2 b)
        {
            return a.Value == b;
        }
        public static bool operator !=(Vector2WithCallback a, Vector2 b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(Vector2 a, Vector2WithCallback b)
        {
            return a == b.Value;
        }
        public static bool operator !=(Vector2 a, Vector2WithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(Vector2WithCallback a, Vector2WithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(Vector2WithCallback a, Vector2WithCallback b)
        {
            if (a.Value == b.Value) return false;

            return true;
        }
    }

    [System.Serializable, InlineProperty]
    public struct Vector3WithCallback : IEquatable<Vector3>
    {
        [SerializeField, HideInInspector]
        private Vector3 _value;

        public Vector3WithCallback(Vector3 startingValue) : this()
        {
            Value = startingValue;
        }

        [ShowInInspector, HideLabel]
        public Vector3 Value
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

        public void AddListener(System.Action<Vector3> listener)
        {
            onValue += listener;
        }

        public void RemoveListener(System.Action<Vector3> listener)
        {
            onValue -= listener;
        }

        [SerializeField, HideInInspector]
        private event Action<Vector3> onValue;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Vector3 other)
        {
            return _value == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vector3WithCallback a, Vector3 b)
        {
            return a.Value == b;
        }
        public static bool operator !=(Vector3WithCallback a, Vector3 b)
        {
            return !(a.Value == b);
        }

        public static bool operator ==(Vector3 a, Vector3WithCallback b)
        {
            return a == b.Value;
        }
        public static bool operator !=(Vector3 a, Vector3WithCallback b)
        {
            return !(a == b.Value);
        }

        public static bool operator ==(Vector3WithCallback a, Vector3WithCallback b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(Vector3WithCallback a, Vector3WithCallback b)
        {
            if (a.Value == b.Value) return false;

            return true;
        }
    }
}
