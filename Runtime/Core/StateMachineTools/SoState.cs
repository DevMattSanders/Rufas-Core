using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class SoState : ScriptableObject
    {
        //  public SoStateMachine stateMachine;

        public UnityEvent<SoState> stateEnterRequested = new UnityEvent<SoState>();

        public UnityEvent<bool> onStateActiveChanged = new UnityEvent<bool>();

        //COMMENT//--Events for enter or exit changed

        private bool stateActive;

        public bool StateActive()
        {
            return stateActive;
        }

        //This is called from the state machine on the current and previously assigned states
        internal void SyncToStateMachine(SoStateMachine stateMachine)
        {
            if (stateMachine == null) return;

            if (stateMachine.nextState == this)
            {
                stateActive = true;
                onStateActiveChanged.Invoke(stateActive);
            }
            else if (stateMachine.previousState == this)
            {
                stateActive = false;
                onStateActiveChanged.Invoke(stateActive);
            }
       
        }

        [Button]
        public void EnterState()
        {
            if (stateActive == true) return;

            stateEnterRequested.Invoke(this);
        }        
    }
}