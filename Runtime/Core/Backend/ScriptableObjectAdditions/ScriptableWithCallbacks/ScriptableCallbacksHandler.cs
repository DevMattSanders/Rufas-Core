using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class ScriptableCallbacksHandler : GameSystem<ScriptableCallbacksHandler>
    {

        [SerializeField,ReadOnly] private ScriptableWithCallbacks[] scriptablesWithCallbacks;

        public override bool IsRufasSystem()
        {
            return true;
        }

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

            TriggerAll_SoOnAwake();
        }

        public override void OnEnable_EditorModeOnly()
        {
            base.OnEnable_EditorModeOnly();
            RefreshCallbackScriptables();
        }

        [Button]
        public void RefreshCallbackScriptables()
        {
#if UNITY_EDITOR
            scriptablesWithCallbacks = RufasStatic.GetAllScriptables_ToArray<ScriptableWithCallbacks>();
#endif
        }

        public override void PostInitialisationBehaviour()
        {
            base.PostInitialisationBehaviour();
            TriggerAll_SoOnStart();
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
