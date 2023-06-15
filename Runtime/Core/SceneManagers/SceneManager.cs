using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Scene/SoSceneManager")]
    public class RufasSceneManager : SuperScriptable
    {
        public static RufasSceneManager instance;

        public string currentScene;
        public string sceneToLoadNext;
        public List<string> currentOpenScenes = new List<string>();

        //[Header("Options")]
        [FoldoutGroup("SceneLoadManager_Options")] public bool automaticallyListenToScreenFaderFadeAmount = true;

        [FoldoutGroup("SceneLoadManager_Options")]
        [HideIf("automaticallyListenToScreenFaderFadeAmount")]
        public float delayBeforeTriggeringSceneChange = 0.5f;

        public BoolWithEvent isCurrentlyLoadingScene;

        public struct BoolWithEvent
        {
            public bool _value;
            public bool Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    if (value == _value)
                    {
                        return;
                    }

                    _value = value;
                    boolEvent.Invoke(_value);
                }
            }

            public UnityEvent<bool> boolEvent;


        }

        //  [FoldoutGroup("SceneLoadManager_Variables")][Required] public BoolVariable isCurrentlyLoadingScene;
        // [FoldoutGroup("SceneLoadManager_Variables")][Required] public FloatVariable sceneLoadPercentAmount;
        //   [FoldoutGroup("SceneLoadManager_Variables")][Required] public BoolVariable isRequestingShowLoadingScreen;



        //   [FoldoutGroup("SceneLoadManager_Events")] public UnityEngine.Events.UnityEvent<string, string> sceneCalledFromScene = new UnityEngine.Events.UnityEvent<string, string>();

        //[FoldoutGroup("SceneLoadManager_Events")][Required] public VoidEvent onSceneLoadTriggered;
        //[FoldoutGroup("SceneLoadManager_Events")][Required] public StringEvent onEnterScene;
        //[FoldoutGroup("SceneLoadManager_Events")][Required] public VoidEvent onExitScene;

        //public 





        public AsyncOperation operation;

        private IEnumerator waitingForSceneTransitionBehaviourRoutine;
        private IEnumerator loadingStatsRoutine;

        [ReadOnly, SerializeField] private bool addressableSceneOpen = false;

        public void OnEnable()
        {

#if UNITY_EDITOR
            /*
            
            string newName = "";
            if (isCurrentlyLoadingScene == null)
            {
                isCurrentlyLoadingScene = ScriptableObject.CreateInstance<BoolVariable>();
                newName = nameof(isCurrentlyLoadingScene);
                AssetDatabase.CreateAsset(isCurrentlyLoadingScene, "Assets/" + newName + ".asset");
                isCurrentlyLoadingScene.name = newName;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            if (sceneLoadPercentAmount == null)
            {
                sceneLoadPercentAmount = ScriptableObject.CreateInstance<FloatVariable>();
                newName = nameof(sceneLoadPercentAmount);
                AssetDatabase.CreateAsset(sceneLoadPercentAmount, "Assets/" + newName + ".asset");
                sceneLoadPercentAmount.name = newName;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
              
            }


            if (showLoadingScreen == null)
            {
                showLoadingScreen = ScriptableObject.CreateInstance<BoolVariable>();
                newName = nameof(showLoadingScreen);
                AssetDatabase.CreateAsset(showLoadingScreen, "Assets/" + newName + ".asset");
                showLoadingScreen.name = newName;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();                
            }
            */
#endif

        }



        public override void SoOnAwake()
        {
            base.SoOnAwake();

            addressableSceneOpen = false;

            currentScene = null;
            currentOpenScenes.Clear();

            GetCurrentScenes_EditorOnly();
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();

            currentScene = null;
            addressableSceneOpen = false;
            currentOpenScenes.Clear();
        }

        private void GetCurrentScenes_EditorOnly()
        {
#if UNITY_EDITOR

            currentScene = null;
            currentOpenScenes.Clear();

            int countLoaded = SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[countLoaded];

            for (int i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
                currentOpenScenes.Add(loadedScenes[i].name);
            }

            if (currentOpenScenes.Count > 0)
            {
                currentScene = currentOpenScenes[0];
            }
#else
        currentScene = SceneManager.GetActiveScene().name;
#endif
        }

        public void LoadScene(SceneReference scene)
        {
            LoadScene(scene.SceneName);
        }
        public void LoadScene(string scene)
        {

        }

        /*
        public void LoadScene(string scene)
        {
            //  Debug.Log("SCENE: " + scene);


            if (string.IsNullOrEmpty(scene)) return;

            if (currentScene != null && currentScene == scene)
            {
                //  Debug.Log("Same Scene!");
                return;
            }

            if (currentOpenScenes.Contains(scene))
            {
                //  Debug.Log("Open Contains!");
                return;
            }

            if (isCurrentlyLoadingScene != null)
            {
                if (isCurrentlyLoadingScene.Value == true)
                {
                    //    Debug.Log("Loading Still!");
                    return;
                }
            }

            if (onSceneLoadTriggered != null) onSceneLoadTriggered.Raise();
            sceneCalledFromScene.Invoke(currentScene, scene);

            if (sceneLoadPercentAmount != null)
            {
                sceneLoadPercentAmount.Value = 0;
            }

            if (isCurrentlyLoadingScene != null)
            {
                isCurrentlyLoadingScene.Value = true;
            }

            sceneToLoadNext = scene;

            TriggerEndOfSceneBehaviours();


        }

        private void TriggerEndOfSceneBehaviours()
        {
            onExitScene.Raise();

            if (waitingForSceneTransitionBehaviourRoutine != null)
            {
                CoroutineMonoBehaviour.Instance.StopCoroutine(waitingForSceneTransitionBehaviourRoutine);
            }

            waitingForSceneTransitionBehaviourRoutine = WaitingForSceneTransitionBehaviour();

            // Debug.Log("2");

            CoroutineMonoBehaviour.Instance.StartCoroutine(waitingForSceneTransitionBehaviourRoutine);
        }



        private IEnumerator WaitingForSceneTransitionBehaviour()
        {
            yield return null;
            //Debug.Log("3");
            BeginTransition();
        }



        private void BeginTransition()
        {
            //  Debug.Log("4");
            //HERE--      UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(loadingScene.SceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive, true).Completed += LoadingSceneActive;
            LoadingSceneActive(new AsyncOperationHandle<SceneInstance>());


        }

        private void LoadingSceneActive(AsyncOperationHandle<SceneInstance> _loadingScene)
        {

            // Debug.Log("5");
            if (isRequestingShowLoadingScreen != null)
            {
                isRequestingShowLoadingScreen.Value = true;
            }


            if (delayToRemoveCurrentSceneRoutine != null)
            {
                CoroutineMonoBehaviour.Instance.StopCoroutine(delayToRemoveCurrentSceneRoutine);
            }

            delayToRemoveCurrentSceneRoutine = DelayToRemoveCurrentScene();

            CoroutineMonoBehaviour.Instance.StartCoroutine(delayToRemoveCurrentSceneRoutine);


        }

        IEnumerator delayToRemoveCurrentSceneRoutine;

        IEnumerator DelayToRemoveCurrentScene()
        {
            // Debug.Log("6");
            if (automaticallyListenToScreenFaderFadeAmount)
            {
                while (ScreenFadeManager.Instance.screenFullyFadedToBlack == false)
                {
                    //  Debug.Log("Waiting for black screen");
                    yield return null;
                }

                CheckAndRemoveCurrentScene();
            }
            else
            {
                yield return new WaitForSeconds(delayBeforeTriggeringSceneChange);


                //HERE   CoroutineMonoBehaviour.Instance.CallWithDelay(CheckAndRemoveCurrentScene, delayBeforeTriggeringSceneChange);
                LoadInNextScene();



            }

        }


        private void CheckAndRemoveCurrentScene()
        {

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScene.SceneName));

            //REMOVES FIRST SCENE (IF STILL ACTIVE)
            if (SceneManager.GetSceneByBuildIndex(0).IsValid() == true)
            {
                //  SceneManager.UnloadSceneAsync(0,UnloadSceneOptions.None);
            }

            foreach (string next in currentOpenScenes)
            {
                SceneManager.UnloadSceneAsync(next);
            }

            currentOpenScenes.Clear();

            // onScreenBlack.Raise();

            //  Debug.Log("7");
            LoadInNextScene();
        }

        private void LoadInNextScene()
        {
            //HERE  operation = SceneManager.LoadSceneAsync(sceneToLoadNext, LoadSceneMode.Additive);
            currentOpenScenes.Clear();
            SceneManager.LoadScene(sceneToLoadNext);


            //  Debug.Log("8");

            StartLoadingStats();


        }



        private void StartLoadingStats()
        {
            if (loadingStatsRoutine != null)
            {
                CoroutineMonoBehaviour.Instance.StopCoroutine(loadingStatsRoutine);
            }

            loadingStatsRoutine = LoadingStats();

            //  Debug.Log("9");

            CoroutineMonoBehaviour.Instance.StartCoroutine(loadingStatsRoutine);
        }



        IEnumerator LoadingStats()
        {
            //HERE
            
         //   while (true)
         //   {
           //     if (sceneLoadPercentAmount != null)
         //       {
          //          sceneLoadPercentAmount.Value = operation.progress;
           //     }
        //
           ////     if (operation.isDone)
           //     {
        //
            //        break;
           //     }
        //
           //     yield return null;
           // }
            
            yield return null;


            addressableSceneOpen = true;
            currentScene = sceneToLoadNext;
            onEnterScene.Raise(currentScene);
            //  Debug.Log("10");
            currentOpenScenes.Add(currentScene);
            //HERE   SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
            //HERE     SceneManager.UnloadSceneAsync(loadingScene.SceneName).completed += Finished;
            //yield return new WaitForSeconds(0.25f);
            Finished(null);
        

        }


        private void Finished(AsyncOperation obj)
        {

            if (isRequestingShowLoadingScreen != null)
            {
                isRequestingShowLoadingScreen.Value = false;
            }


            if (isCurrentlyLoadingScene != null)
            {
                isCurrentlyLoadingScene.Value = false;
            }
        }
        */
    }
}
