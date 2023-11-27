using BrainFailProductions.PolyFew.AsImpL;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Windows;

namespace Rufas

{
    //[CreateAssetMenu(menuName = "Rufas/LoadBeforeStart/AssetListToLoad")]
    //[ExecuteInEditMode]
    //[initial]
    public class RufasBootstrapper : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, ReadOnly] private string group_name = "BootstrappedBehaviours";
   //     public AddressableAssetGroup preAwakeGroup;
#endif
        [SerializeField, ReadOnly] private string labelName = "BootstrappedBehaviour";


        public static AsyncOperationHandle<IList<BootstrapBehaviour>> loadOp;
        public bool loadAddressablesAsIfInABuild = false;

#if UNITY_EDITOR
        [Button]

        public void RebuildGroup()
        {
            if(EditorUtility.DisplayDialog("Confirm Rebuild Group?","This will require an addressables rebuild","Yes, rebuild", "No, cancel"))
            {
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

                AddressableAssetGroup g = settings.FindGroup(group_name);
                settings.RemoveGroup(g);

                RefreshAddressableBootstrapBeheaviours();
            }
        }


        [InitializeOnLoadMethod]
        public static void CreateIfNotFound()
        {
           // Debug.Log("Loaded!");

            RufasBootstrapper[] allBootstrappers = RufasStatic.GetAllScriptables_ToArray<RufasBootstrapper>();
            if(allBootstrappers.Length == 0)
            {
                RufasBootstrapper rufasBootstrapper = ScriptableObject.CreateInstance<RufasBootstrapper>();
                Directory.CreateDirectory("Assets/Rufas/Resources");

                string path = "Assets/Rufas/Resources/RufasBootstrapper.asset";
                AssetDatabase.CreateAsset(rufasBootstrapper, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

             //   rufasBootstrapper.RefreshAddressableBootstrapBeheaviours();
            }
        }

#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void FindAndRunBootstrapBehavioursActivator()
        {
            RufasBootstrapper[] rufasBootstrappers = Resources.LoadAll<RufasBootstrapper>("");

            if (rufasBootstrappers.Length > 1) Debug.LogError("Multiple Rufas Bootstrappers Found! Only initializing first one found");

            rufasBootstrappers[0].LoadAddressablesAndRunBootstrapBehaviours();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            RefreshAddressableBootstrapBeheaviours();
#endif
        }

#if UNITY_EDITOR
        [Button]
        private void RefreshAddressableBootstrapBeheaviours()
        {
            BootstrapBehaviour[] bootstrapBehaviours = RufasStatic.GetAllScriptables_ToArray<BootstrapBehaviour>();
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            foreach (BootstrapBehaviour bootstrapBehaviour in bootstrapBehaviours)
            {
                SetAddressableGroup(bootstrapBehaviour, group_name,labelName);
            }
        }

        public static void SetAddressableGroup(Object obj, string groupName,string labelName)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            if (settings)
            {
                settings.AddLabel(labelName);

                var group = settings.FindGroup(groupName);
                if (!group)
                    group = settings.CreateGroup(groupName, false, true, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

                var assetpath = AssetDatabase.GetAssetPath(obj);
                var guid = AssetDatabase.AssetPathToGUID(assetpath);

                var e = settings.CreateOrMoveEntry(guid, group, true, false);
                e.SetLabel(labelName, true);
                var entriesAdded = new List<AddressableAssetEntry> { e };

                group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
            }
        }
#endif


        public void LoadAddressablesAndRunBootstrapBehaviours()
        {
            bool loadAddressables = true;

#if UNITY_EDITOR
            if (loadAddressablesAsIfInABuild)
            {
                loadAddressables = false;
            }
#endif

            List<BootstrapBehaviour> bootstrapBehaviours = new List<BootstrapBehaviour>();

            if (loadAddressables)
            {                
                loadOp = Addressables.LoadAssetsAsync<BootstrapBehaviour>(labelName, null);

                CoroutineMonoBehaviour.StartCoroutine(LoadOpCounter(), LoadOpCounterRoutine);

                loadOp.Completed += bootstrapBehaviourResults =>
                {
                    AssetsLoaded((List<BootstrapBehaviour>)bootstrapBehaviourResults.Result);
                };
            }
            else
            {
#if UNITY_EDITOR
                AssetsLoaded(RufasStatic.GetAllScriptables_ToList<BootstrapBehaviour>());
#endif
            }          
        }

        private IEnumerator LoadOpCounterRoutine;
        IEnumerator LoadOpCounter()
        {
            while (loadOp.IsDone == false)
            {
              //  Debug.Log(loadOp.PercentComplete);
                yield return null;
            }
        }

        private void AssetsLoaded(List<BootstrapBehaviour> bootstrapBehaviours)
        {
            Debug.Log("Found all behaviours");
            foreach (BootstrapBehaviour next in bootstrapBehaviours)
            {
                Debug.Log(next.name);
                next.BehaviourToRunBeforeStart();
            }
        }
    }
}

