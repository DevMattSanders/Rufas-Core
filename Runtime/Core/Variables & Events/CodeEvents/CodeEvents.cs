using Sirenix.OdinInspector;
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

}