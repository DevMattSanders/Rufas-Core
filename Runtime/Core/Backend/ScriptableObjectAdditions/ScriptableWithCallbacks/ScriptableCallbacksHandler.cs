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

#if UNITY_EDITOR
        public override SdfIconType EditorIcon()
        {
            return SdfIconType.TelephoneForwardFill;
        }
#endif
        public override string DesiredPath()
        {
            return "Rufas/Framework/Scriptable Callbacks";
        }

        // public override bool AutogenerateGameSystem()
        // {
        //      return true;
        //  }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();
            RefreshCallbackScriptables();

       //     GameObject monobehaviourLink = new GameObject("ScriptableCallbacksMonobehaviourLink");

       //     DontDestroyOnLoad(monobehaviourLink);

        //    monobehaviourLink.AddComponent<ScriptableCallbacksMonobehaviourLink>();

          //  TriggerAll_SoOnAwake();
        }

        [Button]
        public void RefreshCallbackScriptables()
        {
#if UNITY_EDITOR
            scriptablesWithCallbacks = RufasStatic.GetAllScriptables_ToArray<ScriptableWithCallbacks>();
#endif
        }

        public override void OnAwakeBehaviour()
        {
            base.OnAwakeBehaviour();
            TriggerAll_SoOnAwake();
        }

        public override void OnStartBehaviour()
        {
            base.OnStartBehaviour();
            TriggerAll_SoOnStart();
        }

        public override void EndOfApplicaitonBehaviour()
        {
            base.EndOfApplicaitonBehaviour();
            TriggerAll_SoOnEnd();
        }

        public override void OnEnable_EditorModeOnly()
        {
            base.OnEnable_EditorModeOnly();
            RefreshCallbackScriptables();
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
