using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [DisableInEditorMode]
    [System.Serializable]
    [InlineProperty]
    public struct CodeEvent
    {
        private event System.Action onEvent;

        public void AddListener(System.Action listener)
        {
            onEvent += listener;
        }

        public void RemoveListener(System.Action listener)
        {
            onEvent -= listener;
        }

        [Button]
        public void Raise()
        {
            onEvent?.Invoke();
        }
    }

    [DisableInEditorMode]
    [InlineProperty]
    [System.Serializable]
    public struct CodeEvent<T>
    {
        private event System.Action<T> onEvent;

        public void AddListener(System.Action<T> listener)
        {
            onEvent += listener;
        }

        public void RemoveListener(System.Action<T> listener)
        {
            onEvent -= listener;
        }
#if UNITY_EDITOR
        [HorizontalGroup("H",order: 0)]
        [SerializeField, HideLabel]
        [InlineButton("RaiseDebug","Raise")]
        private T debug;

        private void RaiseDebug()
        {
            Raise(debug);
        }

        [HorizontalGroup("H",order: 2)]
        [SerializeField, HideLabel,ReadOnly] private T last;
#endif
        [Button]
        public void Raise(T argument)
        {
#if UNITY_EDITOR
            last = argument;
#endif
            onEvent?.Invoke(argument);
        }

        public void AddListener()
        {
            throw new NotImplementedException();
        }
    }

    [DisableInEditorMode]
    [InlineProperty]
    [System.Serializable]
    public struct CodeEvent<T, Y>
    {
        private event System.Action<T, Y> onEvent;

        public void AddListener(System.Action<T, Y> listener)
        {
            onEvent += listener;
        }

        public void RemoveListener(System.Action<T, Y> listener)
        {
            onEvent -= listener;
        }
#if UNITY_EDITOR
        [HorizontalGroup("H", order: 0)]
        [SerializeField, HideLabel]
        private T debugOne;

        [HorizontalGroup("H", order: 1)]
        [SerializeField, HideLabel]
        [InlineButton("RaiseDebug", "Raise")]
        private Y debugTwo;

        private void RaiseDebug()
        {
            Raise(debugOne,debugTwo);
        }
        [HorizontalGroup("H", order: 2)]
        [SerializeField, HideLabel, ReadOnly] public T lastTArgument;

        [HorizontalGroup("H", order: 3)]
        [SerializeField, HideLabel, ReadOnly] public Y lastYArgument;
#endif
        public void Raise(T tArgument, Y yArgument)
        {
#if UNITY_EDITOR
            lastTArgument = tArgument;
            lastYArgument = yArgument;
#endif
            onEvent?.Invoke(tArgument, yArgument);
        }
    }

    [DisableInEditorMode]
    [InlineProperty]
    [System.Serializable]
    public struct CodeEvent<T, U, V>
    {
        private event Action<T, U, V> onEvent;

        public void AddListener(Action<T, U, V> listener)
        {
            onEvent += listener;
        }

        public void RemoveListener(Action<T, U, V> listener)
        {
            onEvent -= listener;
        }

#if UNITY_EDITOR
        [HorizontalGroup("H", order: 0)]
        [SerializeField, HideLabel]
        private T debugOne;

        [HorizontalGroup("H", order: 1)]
        [SerializeField, HideLabel]
        private U debugTwo;

        [HorizontalGroup("H", order: 2)]
        [SerializeField, HideLabel]
        [InlineButton("RaiseDebug", "Raise")]
        private V debugThree;

        private void RaiseDebug()
        {
            Raise(debugOne, debugTwo, debugThree);
        }

        [HorizontalGroup("H", order: 3)]
        [SerializeField, HideLabel, ReadOnly] public T lastTArgument;

        [HorizontalGroup("H", order: 4)]
        [SerializeField, HideLabel, ReadOnly] public U lastUArgument;

        [HorizontalGroup("H", order: 5)]
        [SerializeField, HideLabel, ReadOnly] public V lastVArgument;
#endif

        public void Raise(T tArgument, U uArgument, V vArgument)
        {
#if UNITY_EDITOR
            lastTArgument = tArgument;
            lastUArgument = uArgument;
            lastVArgument = vArgument;
#endif
            onEvent?.Invoke(tArgument, uArgument, vArgument);
        }
    }

}