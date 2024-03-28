using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class MonoStateMachine : MonoBehaviour
    {
        [ReadOnly]
        public MonoState currentState;

       // public MonoState previousState { get; private set; }
       // public MonoState nextState { get; private set; }

        public MonoState startingState;

        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true)]
        public List<MonoState> states = new List<MonoState>();

#if UNITY_EDITOR
        [Button]
        public void CreateNewState(string stateName)
        {
            MonoState newState = new GameObject(stateName).AddComponent<MonoState>();
            newState.transform.SetParent(transform);
            states.Add(newState);
            newState.stateMachine = this;
        }
#endif

        private void Awake()
        {
            currentState = null;
          //  nextState = null;
          //  previousState = null;

            foreach(MonoState nextState in states)
            {
                if(nextState.transform.parent != transform)
                {
                    Debug.LogError("MonoState assigend under wrong state machine! Need to create them using the CreateNewState button!");
                }

                nextState.IsCurrentState.Value = false;
            }

            startingState.SetAsCurrentState();
        }

        public void SetState(MonoState state)
        {
            if(state == null && currentState != null)
            {
                currentState.IsCurrentState.Value = false;
                currentState = null;
                return;
            }

          
            if(currentState == state)
            {
                state.IsCurrentState.Value = true;
                return;
            }

            if(currentState != null)
            {
                currentState.IsCurrentState.Value = false;               
            }

            currentState = state;
            state.IsCurrentState.Value = true;
        }
    }
}
