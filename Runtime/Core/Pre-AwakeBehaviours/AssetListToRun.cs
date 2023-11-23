using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas

{
    [CreateAssetMenu(menuName = "Rufas/LoadBeforeStart/AssetListToLoad")]
    public class AssetListToRun : ScriptableObject
    {
        public List<PreAwakeBehaviour> preAwakeBehaviours = new List<PreAwakeBehaviour>();


        private void OnEnable()
        {
#if UNITY_EDITOR
            preAwakeBehaviours = RufasStatic.GetAllScriptables_ToList<PreAwakeBehaviour>();
#endif
        }

        public void Run()
        {
            Debug.Log("Running Before Scene Load Behaviour");
            foreach(PreAwakeBehaviour next in preAwakeBehaviours)
            {
                next.BehaviourToRunBeforeStart();
            }
        }
    }
}
