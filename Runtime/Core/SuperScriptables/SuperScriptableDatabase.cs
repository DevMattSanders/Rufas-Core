using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/SuperScriptableDatabase")]
    public class SuperScriptableDatabase : PreAwakeBehaviour
    {
        public List<string> exceptions = new List<string>();

        public List<SuperScriptable> superScriptables = new List<SuperScriptable>();

        public override void BehaviourToRunBeforeStart()
        {
            base.BehaviourToRunBeforeStart();

            GameObject monobehaviourLink = new GameObject("SuperScriptableMonobehaviourEvents");

            DontDestroyOnLoad(monobehaviourLink);

            monobehaviourLink.AddComponent<SuperScriptableMonobehaviourEvents>();
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
            superScriptables = RufasStatic.GetAllScriptables_ToList<SuperScriptable>();
#endif
        }


        public void TriggerAll_SoOnAwake()
        {
            exceptions.Clear();

#if UNITY_EDITOR

            FindAllSuperScriptables();
#endif

            foreach (var superScriptable in superScriptables)
            {
                try
                {
                    Debug.Log("Awake: " + superScriptable.name);
                    superScriptable.SoOnAwake();
                }
                catch (System.Exception ex)
                {
                    exceptions.Add("OnAwake(" + superScriptable.GetType() + ")_" + superScriptable.name + "_" + ex.Message+ " " + ex.Source);
                    Debug.Log("OnAwake(" + superScriptable.GetType() + ")_" + superScriptable.name + "_" + ex.Message + " " + ex.Source);
                   // throw;
                }
           
            }
        }

        public void TriggerAll_SoOnStart()
        {
            foreach (var superScriptable in superScriptables)
            {
                try
                {
                    superScriptable.SoOnStart();
                }
                catch (System.Exception ex)
                {
                    exceptions.Add("OnStart(" + superScriptable.GetType() + ")_" + superScriptable.name + "_" + ex.Message);
                    Debug.Log("OnStart(" + superScriptable.GetType() + ")_" + superScriptable.name + "_" + ex.Message);
                    throw;
                }

            }
        }

        public void TriggerAll_SoOnEnd()
        {
            foreach (var superScriptable in superScriptables) { superScriptable.SoOnEnd(); }

        }
    }
}
