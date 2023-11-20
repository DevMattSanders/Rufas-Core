using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class ScriptableCallbacksHandler : GameSystem<ScriptableCallbacksHandler>
    {

        public ScriptableWithCallbacks[] scriptablesWithCallbacks;

        /*
        public override bool RufasBackendSystem()
        {
            return true;
        }
        */
        public override bool AutogenerateGameSystem()
        {
            return true;
        }

        public override void BehaviourToRunBeforeAwake()
        {
            base.BehaviourToRunBeforeAwake();
            RefreshCallbackScriptables();

            GameObject monobehaviourLink = new GameObject("ScriptableCallbacksMonobehaviourLink");

            DontDestroyOnLoad(monobehaviourLink);

            monobehaviourLink.AddComponent<ScriptableCallbacksMonobehaviourLink>();
        }

        private void OnEnable()
        {
            RefreshCallbackScriptables();
        }

        [Button]
        public void RefreshCallbackScriptables()
        {
#if UNITY_EDITOR
            scriptablesWithCallbacks = RufasStatic.GetAllScriptables_ToArray<ScriptableWithCallbacks>();
#endif
        }

        public void TriggerAll_SoOnAwake()
        {
            foreach(ScriptableWithCallbacks next in scriptablesWithCallbacks)
            {
                next.SoOnAwake();
            }
        }

        public void TriggerAll_SoOnStart()
        {
            foreach (ScriptableWithCallbacks next in scriptablesWithCallbacks)
            {
                next.SoOnStart();
            }
        }

        public void TriggerAll_SoOnEnd()
        {
            foreach (ScriptableWithCallbacks next in scriptablesWithCallbacks)
            {
                next.SoOnEnd();
            }
        }


    }
}
