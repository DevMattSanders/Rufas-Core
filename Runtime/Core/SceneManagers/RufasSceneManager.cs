using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Rufas
{
    public class RufasSceneManager : GameSystem<RufasSceneManager>
    {
        [FoldoutGroup("Application Scenes")]
        [SerializeField]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = false, HideRemoveButton = true, HideAddButton = true)]
        public List<SceneInfo> applicationScenes = new List<SceneInfo>();
        [PropertySpace(15)]
        [FoldoutGroup("Debug")] public CodeEvent onSceneLoadTriggered;
        [FoldoutGroup("Debug")] public CodeEvent onSceneLoadCompleted;
        [FoldoutGroup("Debug"), ReadOnly] public BoolWithCallback isCurrentlyLoadingScene;
        [FoldoutGroup("Debug"), ReadOnly] public FloatWithCallback sceneLoadPercent;

        [FoldoutGroup("Current Snapshot")] public string lastOpenedScene;
        [FoldoutGroup("Current Snapshot")][ReadOnly, SerializeField] private List<string> nonAddressableOpenScenes = new List<string>();
        [FoldoutGroup("Current Snapshot")][ReadOnly, SerializeField, SerializeReference] private AssetReference queuedSceneToLoad;
        [FoldoutGroup("Current Snapshot")][ReadOnly, SerializeField, SerializeReference] public List<SceneInstance> openScenes = new List<SceneInstance>();

        public string GetCurrentScene()
        {
            if (isCurrentlyLoadingScene.Value)
            {
                return loadingScene;
            }
            else
            {
                if(lastOpenedScene == "")
                {
                    lastOpenedScene = SceneManager.GetActiveScene().name;
                }
                return lastOpenedScene;
            }
        }

        public static void LoadScene(AssetReference sceneInstance)
        {
            RufasSceneManager.Instance.LoadSceneFromInstance(sceneInstance);
        }

        [HorizontalGroup("Debug/H")][ReadOnly, SerializeField, HideInInspector] public string firstScene { get; private set; }
#if UNITY_EDITOR
        [HorizontalGroup("Debug/H")][ReadOnly, SerializeField, ShowInInspector] public SceneAsset firstSceneAsset;
#endif
        [HorizontalGroup("Debug/H")][ReadOnly, SerializeField, HideInInspector] public string loadingScene { get; private set; }
#if UNITY_EDITOR
        [HorizontalGroup("Debug/H")][ReadOnly, SerializeField, ShowInInspector] public SceneAsset loadingSceneAsset;
#endif
        

        [ReadOnly, HideInEditorMode]
        public List<Object> sceneLoadStallers = new List<Object>();

        [ReadOnly, HideInEditorMode, SerializeReference]
        public List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        [ReadOnly, HideInEditorMode, SerializeReference]
        public List<AsyncOperationHandle> asyncOperationHandles = new List<AsyncOperationHandle>();

        [FoldoutGroup("Fade to black")]
        [Tooltip("Loaded on start!")] //Inline button if null for creating one?
        public AssetReference screenFadePrefab;
        [FoldoutGroup("Fade to black")]
        [ShowIf("screenFadePrefab")]
        public float fadeToBlackDuration = 0.2f;


#if UNITY_EDITOR
        public override SdfIconType EditorIcon()
        {
            return SdfIconType.CameraReels;
        }
#endif
        public override string DesiredName()
        {
            return "Scene Manager";

        }
        public override string DesiredPath()
        {
            return "Rufas/Scene Manager";
        }

        public override void PreInitialisationBehaviour() { base.PreInitialisationBehaviour(); ResetValues(); }

        public override void FinaliseInitialisation()
        {
            if (screenFadePrefab == null)
            {
                base.FinaliseInitialisation();
            }
            else
            {
                Addressables.LoadAssetAsync<GameObject>(screenFadePrefab).Completed += asset =>
                {
                    if (asset.Result != null) DontDestroyOnLoad(GameObject.Instantiate(asset.Result));
                    base.FinaliseInitialisation();

                };
            }
        }

        public override void OnAwakeBehaviour() { base.OnAwakeBehaviour(); nonAddressableOpenScenes.Add(SceneManager.GetActiveScene().name); }
        public override void EndOfApplicaitonBehaviour() { base.EndOfApplicaitonBehaviour(); ResetValues(); }
        public override void OnEnable_EditorModeOnly()
        {
            base.OnEnable_EditorModeOnly();

#if UNITY_EDITOR
            RefreshApplicationScenesList();
#endif
        }

        public void ResetValues()
        {
            sceneLoadPercent.Value = 0;
            isCurrentlyLoadingScene.Value = false;
            nonAddressableOpenScenes.Clear();
            queuedSceneToLoad = null;
            lastOpenedScene = "";
            openScenes.Clear();
        }

        public void LoadSceneFromInstance(AssetReference asset)
        {
            if (asset == null || isCurrentlyLoadingScene.Value) return;

            bool found = false;

            foreach (SceneInfo next in applicationScenes)
            {
                // Debug.Log(next.sceneAssetReference.AssetGUID);
                if (string.Compare(asset.RuntimeKey.ToString(), next.sceneAssetReference.RuntimeKey.ToString()) == 0)
                {
                    found = true;
                }
            }

            if (found == false) return;


            onSceneLoadTriggered.Raise();
            sceneLoadPercent.Value = 0;
            isCurrentlyLoadingScene.Value = true;
            queuedSceneToLoad = asset;

            CoroutineMonoBehaviour.StartCoroutine(TransitionToLoadingSceneRoutine(), _transitionToLoadingSceneRoutine);
        }

        private IEnumerator _transitionToLoadingSceneRoutine; private IEnumerator TransitionToLoadingSceneRoutine()
        {
            //Waiting for scene load stallers (such as fade to black handler)
            while (sceneLoadStallers.Count > 0) { if (sceneLoadStallers.Any(next => next == null)) break; yield return null; }


            SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive).completed += _loadingScene =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScene));

                CoroutineMonoBehaviour.StartCoroutine(UnloadAndLoadScenesRoutine(), _UnloadAndLoadScenesRoutine);
            };
        }

        private IEnumerator _UnloadAndLoadScenesRoutine; private IEnumerator UnloadAndLoadScenesRoutine()
        {
            //Loading scene now active. Begin async unload of all active scenes that need removing

            foreach (string next in nonAddressableOpenScenes) asyncOperations.Add(SceneManager.UnloadSceneAsync(next));

            foreach (SceneInstance next in openScenes) asyncOperationHandles.Add(Addressables.UnloadSceneAsync(next, autoReleaseHandle: true));

            while (true)
            {
                if (asyncOperations.Any(next => next.isDone == false) || asyncOperationHandles.Any(next => next.IsDone == false))
                {
                    yield return null;
                }
                else
                {
                    break;
                }

                sceneLoadPercent.Value = 0.5f;
            }

            asyncOperations.Clear();
            asyncOperationHandles.Clear();
            nonAddressableOpenScenes.Clear();
            openScenes.Clear();

            //Now load new scene
            AsyncOperationHandle<SceneInstance> _newScene = Addressables.LoadSceneAsync(queuedSceneToLoad.RuntimeKey, LoadSceneMode.Additive);

            _newScene.Completed += _newScene =>
            {
                lastOpenedScene = _newScene.Result.Scene.name;
                openScenes.Add(_newScene.Result);
                SceneManager.SetActiveScene(_newScene.Result.Scene);
                
                SceneManager.UnloadSceneAsync(loadingScene).completed += _loadingScene =>
                {
                    isCurrentlyLoadingScene.Value = false;
                    sceneLoadPercent.Value = 1f;
                    onSceneLoadCompleted.Raise();
                };
            };

            //Setting percentage
            while (_newScene.IsDone == false && isCurrentlyLoadingScene.Value == true)
            {
                sceneLoadPercent.Value = 0.5f + (_newScene.PercentComplete / 2);
                yield return null;
            }


        }

#if UNITY_EDITOR
        [Button]
        public void RefreshApplicationScenesList()
        {
            applicationScenes.Clear();

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found. Please make sure you have Addressables installed.");
                return;
            }
            List<AddressableAssetEntry> sceneEntries = new List<AddressableAssetEntry>();

            settings.GetAllAssets(sceneEntries, false);

            int sceneListCount = 0;

            SoSceneReference[] references = RufasStatic.GetAllScriptables_ToArray<SoSceneReference>();
            bool editorNeedsRefresh = false;
            foreach (var sceneEntry in sceneEntries)
            {
                if (sceneEntry.IsScene)
                {
                    if (sceneEntry.IsInSceneList)
                    {
                        if (sceneListCount == 0)
                        {
                            firstScene = System.IO.Path.GetFileNameWithoutExtension(sceneEntry.address);
                            firstSceneAsset = sceneEntry.MainAsset as SceneAsset;

                        }

                        if (sceneListCount == 1)
                        {
                            loadingScene = System.IO.Path.GetFileNameWithoutExtension(sceneEntry.address);
                            loadingSceneAsset = sceneEntry.MainAsset as SceneAsset;

                        }

                        sceneListCount++;
                    }
                    else
                    {

                        SceneInfo sceneInfo = new SceneInfo
                        {
                            sceneAssetReference = new AssetReference(sceneEntry.guid)
                        };

                        applicationScenes.Add(sceneInfo);

                        bool found = false;

                        foreach(SoSceneReference next in references)
                        {
                            if(next.sceneReference != null)
                            {
                                if(string.Compare(next.sceneReference.RuntimeKey.ToString(),sceneInfo.sceneAssetReference.RuntimeKey.ToString()) == 0)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }

                        if (found == false)
                        {
                            // Create a new SoSceneReference within the folder Assets/Rufas/Scenes
                            string folderPath = "Assets/Rufas/Scenes";
                            string assetPath = $"{folderPath}/{sceneInfo.sceneAssetReference.editorAsset.name}.asset";

                            SoSceneReference newSoSceneReference = ScriptableObject.CreateInstance<SoSceneReference>();
                            newSoSceneReference.sceneReference = new AssetReference(sceneEntry.guid);

                            // Create the folder if it doesn't exist
                            if (!AssetDatabase.IsValidFolder(folderPath))
                            {
                                AssetDatabase.CreateFolder("Assets/Rufas", "Scenes");
                            }

                            editorNeedsRefresh = true;

                            AssetDatabase.CreateAsset(newSoSceneReference, assetPath);
                            
                        }
                    }
                }
            }

            if (sceneListCount == 0) Debug.LogError("No first scene found in build settings! This should be the first scene in the list");

            if (sceneListCount == 0 || sceneListCount == 1) Debug.LogError("No loading scene found in build settings! This should be the second scene in the list");

            if (sceneListCount > 2) Debug.LogError("More than two scenes found in the build settings! Please update to use addressable scenes");

            if (editorNeedsRefresh)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

#endif

        [System.Serializable]
        public class SceneInfo
        {
            [HideLabel, ReadOnly, HorizontalGroup("H", width: 200)]
            public AssetReference sceneAssetReference;

            [HorizontalGroup("H")]
            [PropertyOrder(0)]
            [Button("$SceneName"), DisableInEditorMode]
            public void Load()
            {
                RufasSceneManager.LoadScene(sceneAssetReference);
            }

            private string SceneName()
            {
#if UNITY_EDITOR
                if (sceneAssetReference == null) return "-No Asset!-";

                if (sceneAssetReference.editorAsset == null) return "-No Asset!-";

                return sceneAssetReference.editorAsset.name;
#else
                return "Load Scene";
#endif

            }
        }

    }
}
