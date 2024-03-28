using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class MonoState : MonoBehaviour
    {
        [ReadOnly]
        public MonoStateMachine stateMachine;

        public BoolWithCallback IsCurrentState;

        public void AddListener(System.Action<bool> listener, bool callNow = false)
        {
            //Add proxy listener
            IsCurrentState.AddListener(listener, callNow);
        }

        public void RemoveListener(System.Action<bool> listener)
        {
            //Remove proxy listener
            IsCurrentState.RemoveListener(listener);
        }

        private bool IsCurrentStateValue()
        {
            return IsCurrentState.Value;
        }

        [HideIf("IsCurrentStateValue")]
        [Button]
        public void SetAsCurrentState()
        {
            stateMachine.SetState(this);
        }
    }
}