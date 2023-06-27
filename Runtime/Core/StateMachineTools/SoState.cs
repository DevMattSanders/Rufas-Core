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

        [HideInInspector]
        public CodeEvent<SoState> stateEnterRequested;// = new UnityEvent<SoState>();

        [ReadOnly]
        public CodeEvent<bool> onStateActiveChanged;// = new UnityEvent<bool>();

        //COMMENT//--Events for enter or exit changed
        [SerializeField,ReadOnly]
        private bool stateActive;

        public bool StateActive
        {
            get
            {
                return stateActive;
            }
            set
            {

                stateActive = value;
            }

        }

        //This is called from the state machine on the current and previously assigned states
        internal void SyncToStateMachine(SoStateMachine stateMachine)
        {
            if (stateMachine == null) return;

            if (stateMachine.nextState == this)
            {
                stateActive = true;
                onStateActiveChanged.Raise(stateActive);
            }
            else if (stateMachine.previousState == this)
            {
                stateActive = false;
                onStateActiveChanged.Raise(stateActive);
            }
            else
            {
                stateActive = false;
            }
       
        }

        [ShowInInlineEditors]
        [GUIColor("ButtonColour")]
        [Button]        
        public void EnterState()
        {
            if (stateActive == true) return;

            stateEnterRequested.Raise(this);
        }

        [Button]
        [HideInInlineEditors]
        [GUIColor("ButtonColour")]
        [ShowInInspector]
        private void EnterState_Debug()
        {
            EnterState();
        }

        private Color ButtonColour()
        {
            if (stateActive) return new Color(0.8f, 1, 0.8f);

             return new Color(1, 0.8f, 0.8f);
        }
    }
}