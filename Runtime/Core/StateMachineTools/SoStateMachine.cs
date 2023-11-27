using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/StateMachine/StateMachine")]
    public class SoStateMachine : SuperScriptable
    {
        //[HideInPlayMode]
        [ReadOnly]
        public SoState currentState;

        public SoState previousState { get; private set; }
        public SoState nextState { get; private set; }

        [HideInInspector]        
        public SoState startingState;

        [ListDrawerSettings(HideRemoveButton = true,HideAddButton = true)]
        public List<SoState> states = new List<SoState>();

#if UNITY_EDITOR
        [Button]
        public void CreateNewState(string stateName)
        {
            SoState newState = ScriptableObject.CreateInstance<SoState>();
            newState.name = $"{name}_{stateName}";

            // Get the path of the parent asset
            string parentPath = AssetDatabase.GetAssetPath(this);
            string assetPath = Path.Combine(Path.GetDirectoryName(parentPath), $"{newState.name}.asset");

            // Save the new asset
            AssetDatabase.CreateAsset(newState, assetPath);
            AssetDatabase.SaveAssets();

            // Add the new asset to the parent's states list
            states.Add(newState);
        }
#endif

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            Debug.Log("So On Awake Called");

            currentState = null;
            nextState = null;
            previousState = null;

            foreach (SoState nextState in states)
            {
                nextState.stateEnterRequested.AddListener(StateEnterRequested);
                nextState.SyncToStateMachine(this);
            }
                       
                      
        }

        public override void SoOnStart()
        {
            base.SoOnStart();

            startingState = states[0];

            startingState.EnterState();
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();

            previousState = null;
            nextState = null;
            currentState = null;

            foreach (SoState nextState in states)
            {
                nextState.stateEnterRequested.RemoveListener(StateEnterRequested);

                if (nextState.StateActive) nextState.SyncToStateMachine(this);
            }
            //startingState.EnterState();

         
        }

        private void StateEnterRequested(SoState state)
        {
            if (currentState == state) return;

            if(currentState != null)
            {
                previousState = currentState;
            }

            nextState = state;

            currentState = state;

            if (previousState != null)
            {
                previousState.SyncToStateMachine(this);
            }
            nextState.SyncToStateMachine(this);

        }
    }
}