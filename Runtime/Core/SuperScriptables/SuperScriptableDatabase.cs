using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/SuperScriptableDatabase")]
    public class SuperScriptableDatabase : PreAwakeBehaviour
    {
        public List<SuperScriptable> superScriptables = new List<SuperScriptable>();

        public override void BehaviourToRunBeforeStart()
        {
            base.BehaviourToRunBeforeStart();

            GameObject monobehaviourLink = new GameObject("SuperScriptableMonobehaviourEvents");

            DontDestroyOnLoad(monobehaviourLink);

            monobehaviourLink.AddComponent<SuperScriptableMonobehaviourEvents>();//.database = this;

            //monobehaviourLink.
        }


        private void OnEnable()
        {
            FindAllSuperScriptables();
        }


        [Button]
        public void FindAllSuperScriptables()
        {
            //_/Debug.Log("Find All Super Scriptables! (Only works in editor!)");
#if UNITY_EDITOR
            superScriptables = GeneralMethods.FindAllScriptableObjectsOfType<SuperScriptable>();
#endif
        }


        public void TriggerAll_SoOnAwake()
        {
#if UNITY_EDITOR

            FindAllSuperScriptables();

#endif

         //   Debug.Log("So Awake");
            foreach (var superScriptable in superScriptables) { superScriptable.SoOnAwake(); }
        }

        public void TriggerAll_SoOnStart()
        {
          //  Debug.Log("So Start");
            foreach (var superScriptable in superScriptables) { superScriptable.SoOnStart(); }
        }

        public void TriggerAll_SoOnEnd()
        {
          //  Debug.Log("So End");
            foreach (var superScriptable in superScriptables) { superScriptable.SoOnEnd(); }
        }
    }
}
