using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace Rufas
{
    public static class TriggerPreAwakeBehaviours
    {
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void FindAndLoadAllPreSceneLoaders()
        {
            //PreAwakeBehaviour[] loadBeforeStartObjects = Resources.LoadAll<PreAwakeBehaviour>("");

            // AssetListToRun assetList = Resources.Load<AssetListToRun>("AssetListToRunResources");

            if (EditorSceneManager.GetActiveScene().name != "FirstScene")
            {

                AssetListToRun[] assetLists = RufasStatic.GetAllScriptables_ToArray<AssetListToRun>();
                assetLists[0].Run();
            }
            else
            {
                Debug.Log("On First Scene. Not loading asset lists");
            }

            /*
            var loadOp = Addressables.LoadAsset<AssetListToRun>("Assets/Rufas/AssetListToRun.asset");

            await loadOp.Task;

            loadOp.Result.Run();
            */

            //Resources.l

          //  var loadOperation = Addressables.LoadAssets<PreAwakeBehaviour>("LoadOnInit", null);

           // var loadOperation = Addressables.LoadAssets<PreAwakeBehaviour>("LoadOnInit", null);
           /*
            AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocations(new string[] { "LoadOnInit" }, Addressables.MergeMode.Union);

            await handle.Task;

            IList<IResourceLocation> resourceLocations = handle.Result;

            foreach(var location in resourceLocations)
            {
                AsyncOperationHandle<PreAwakeBehaviour> assetHandle = Addressables.LoadAsset<PreAwakeBehaviour>(location);

                assetHandle.Result.BehaviourToRunBeforeStart();
            }
           */
          //  Debug.Log(loadOperation.Result.ToArray().Length);

        //    PreAwakeBehaviour[] loadBeforeStartObjects = loadOperation.Result.ToArray();


          //  List<PreAwakeBehaviour> loadBeforeStartList = new List<PreAwakeBehaviour>(loadBeforeStartObjects);



            //Loop through each 'LoadBeforeStart' object and check it's priority.
            //If equal to the priority value, run it's 'BehaviourToRunBeforeStart' method them remove from the array.            

          //  int priority = 0;

           // while (priority < 11)
           // {
            ///    foreach(PreAwakeBehaviour next in loadBeforeStartList)
            //    {
            //        if (next.priority == priority) next.BehaviourToRunBeforeStart();
            //    }
            //    priority++;
          //  }

        }
#endif

    }
}
