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
        [FoldoutGroup("Debug")] public CodeEvent onSceneLoadTriggered;
        [FoldoutGroup("Debug")] public CodeEvent onSceneLoadCompleted;
        [FoldoutGroup("Debug"), ReadOnly] public BoolWithCallback isCurrentlyLoadingScene;
        [FoldoutGroup("Debug"),ReadOnly] public FloatWithCallback sceneLoadPercent;

        [FoldoutGroup("Current Snapshot")][ReadOnly, SerializeField] private List<string> nonAddressableOpenScenes = new List<string>();
        [FoldoutGroup("Current Snapshot")][ReadOnly, SerializeField, SerializeReference] private AssetReference queuedSceneToLoad;
        [FoldoutGroup("Current Snapshot")][ReadOnly, SerializeField, SerializeReference] public List<SceneInstance> openScenes = new List<SceneInstance>();

      public static void LoadScene(AssetReference sceneInstance)
        {
            RufasSceneManager.Instance.LoadSceneFromInstance(sceneInstance);
        }

        [HorizontalGroup("H")]
        [ReadOnly, SerializeField,HideInInspector] public string firstScene { get; private set; }
#if UNITY_EDITOR
        [HorizontalGroup("H")]
        [ReadOnly, SerializeField, ShowInInspector]
        public SceneAsset firstSceneAsset;
#endif
        [HorizontalGroup("H")]
        [ReadOnly,SerializeField,HideInInspector] public string loadingScene { get; private set; }
#if UNITY_EDITOR
        [HorizontalGroup("H")]
        [ReadOnly, SerializeField, ShowInInspector]
        public SceneAsset loadingSceneAsset;
#endif
        [PropertySpace(15)]
        [SerializeField]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = false, HideRemoveButton = true,HideAddButton = true)]
        [InlineButton("RefreshApplicationScenesList")]
        public List<SceneInfo> applicationScenes = new List<SceneInfo>();

        [ReadOnly,HideInEditorMode]
        public List<Object> sceneLoadStallers = new List<Object>();

        [ReadOnly,HideInEditorMode,SerializeReference]
        public List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        [ReadOnly,HideInEditorMode,SerializeReference]
        public List<AsyncOperationHandle> asyncOperationHandles = new List<AsyncOperationHandle>();

        [TitleGroup("Fade to black")]
        [Tooltip("Loaded on start!")] //Inline button if null for creating one?
        public AssetReference screenFadePrefab;
        [TitleGroup("Fade to black")]
        [ShowIf("screenFadePrefab")]
        public float fadeToBlackDuration = 0.2f;

        public override void BehaviourToRunBeforeAwake() { base.BehaviourToRunBeforeAwake(); ResetValues(); }

        public override void FinaliseInitialisation()
        {
            if(screenFadePrefab == null)
            {
                base.FinaliseInitialisation();
            }
            else
            {
                Addressables.LoadAssetAsync<GameObject>(screenFadePrefab).Completed += asset =>
                {
                    if(asset.Result != null) DontDestroyOnLoad(GameObject.Instantiate(asset.Result));

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
            openScenes.Clear();
        }

        public void LoadSceneFromInstance(AssetReference asset)
        {
            if (asset == null || isCurrentlyLoadingScene.Value) return;

            bool found = false;

            foreach(SceneInfo next in applicationScenes)
            {
               // Debug.Log(next.sceneAssetReference.AssetGUID);
                if(string.Compare(asset.RuntimeKey.ToString(),next.sceneAssetReference.RuntimeKey.ToString()) == 0)
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
            while (sceneLoadStallers.Count > 0) { if (sceneLoadStallers.Any(next => next == null)) break; yield return null;}


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

        public void RefreshApplicationScenesList()
        {
            applicationScenes.Clear();

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if(settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found. Please make sure you have Addressables installed.");
                return;
            }       
            List<AddressableAssetEntry> sceneEntries = new List<AddressableAssetEntry>();

            settings.GetAllAssets(sceneEntries,false);

            int sceneListCount = 0;

            foreach(var sceneEntry in sceneEntries)
            {
                if (sceneEntry.IsScene)
                {
                    if (sceneEntry.IsInSceneList)
                    {
                        if (sceneListCount == 0)
                        {
                            firstScene = System.IO.Path.GetFileNameWithoutExtension(sceneEntry.address);
#if UNITY_EDITOR
                            firstSceneAsset = sceneEntry.MainAsset as SceneAsset;
#endif
                        }

                        if (sceneListCount == 1)
                        {
                            loadingScene = System.IO.Path.GetFileNameWithoutExtension(sceneEntry.address);
#if UNITY_EDITOR
                            loadingSceneAsset = sceneEntry.MainAsset as SceneAsset;
#endif
                        }

                        sceneListCount++;
                    }
                    else
                    {
                       // AssetReference newAsset = new AssetReference(sceneEntry.guid);
                       // scenesInApplication.Add(newAsset);

                        applicationScenes.Add(new SceneInfo
                        {
                            sceneAssetReference = new AssetReference(sceneEntry.guid)
                           // sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneEntry.address),
                           // addressableKey = sceneEntry.address
                        });
                    }
                }                
            }

            if (sceneListCount == 0) Debug.LogError("No first scene found in build settings! This should be the first scene in the list");

            if (sceneListCount == 0 || sceneListCount == 1) Debug.LogError("No loading scene found in build settings! This should be the second scene in the list");

            if (sceneListCount > 2) Debug.LogError("More than two scenes found in the build settings! Please update to use addressable scenes");

            
        }

#endif

        [System.Serializable]       
        public class SceneInfo
        {
            [HideLabel,ReadOnly,HorizontalGroup("H",width: 200)]
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
                if (sceneAssetReference == null) return "-No Asset!-";
#if UNITY_EDITOR
                return sceneAssetReference.editorAsset.name;
#endif
                return "Load Scene";

            }
        }

    }
}
