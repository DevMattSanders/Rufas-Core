using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rufas
{
    public class LoadBeforeAwakeHandler : GameSystem<LoadBeforeAwakeHandler>
    {
        [SerializeField,ReadOnly] private LoadBeforeAwake[] loaders;

        public override bool IsRufasSystem()
        {
            return true;
        }


#if UNITY_EDITOR
        public override SdfIconType EditorIcon()
        {
            return SdfIconType.LayerForward;
        }
#endif

        public override string DesiredPath()
        {
            return "Rufas/Framework/Load-Before-All";
        }



        //  public override bool AutogenerateGameSystem()
        //  {
        //       return true;
        //    }

        public override void OnEnable_EditorModeOnly()
        {
            base.OnEnable_EditorModeOnly();
            Refresh();
        }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();

#if UNITY_EDITOR
            Refresh();
#endif

            foreach (LoadBeforeAwake next in loaders)
            {
                next.BehaviourToRunBeforeStart();
            }
        }

        public override void FinaliseInitialisation()
        {
            CoroutineMonoBehaviour.i.StartCoroutine(WaitForAllPreawakeLoadersBeforeFinishInitializing());            
        }

        IEnumerator WaitForAllPreawakeLoadersBeforeFinishInitializing()
        {
            while (true)
            {
                bool finished = true;

                foreach(LoadBeforeAwake next in loaders)
                {
                    foreach(AssetReference nextRef in next.addressablesToLoad)
                    {
                        if (nextRef.IsDone == false)
                        {
                            finished = false;
                         //   Debug.Log(nextRef.SubObjectName + " Not done");
                            break;
                        }
                    }
                }

                if(finished == true)
                {
                    break;
                }

                yield return null;
            }

            base.FinaliseInitialisation();
        }

        [Button]
        private void Refresh()
        {
#if UNITY_EDITOR
            loaders = RufasStatic.GetAllScriptables_ToArray<LoadBeforeAwake>();
#endif
        }
    }
}
