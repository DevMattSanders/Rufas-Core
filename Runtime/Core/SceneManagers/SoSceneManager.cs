using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Scene/SoSceneManager")]
    public class SoSceneManager : SuperScriptable
    {            



        public CodeEvent onSceneLoadTriggered;
        public CodeEvent endOfScene;
        public CodeEvent<string> onSceneEntered;

        public CodeEvent<string, string> sceneCalledFromScene;


        public static SoSceneManager instance;

        public BoolWithCallback isCurrentlyLoadingScene;       
        public BoolWithCallback showLoadingScreen;
        public FloatWithCallback percentAmount;

        [ReadOnly, SerializeField] private string currentScene;
       // [ReadOnly, SerializeField] private SceneInstance currnetActiveScene;
        [ReadOnly, SerializeField] private string sceneToLoadNextName;
        [ReadOnly, SerializeField] private AssetReference sceneToLoadNextAssetReference;

        [ReadOnly,SerializeField] public List<string> currentOpenScenes = new List<string>();

        [ReadOnly,SerializeField] public List<AsyncOperationHandle> currentlyOpenScenes = new List<AsyncOperationHandle>();

        [DisableInPlayMode]
        public LogicGroup sceneLoadingLogicGroup;

        public SceneReference loadingScene;

       // public AsyncOperation operation;

        public AsyncOperationHandle<SceneInstance> operation;

        //Whilst there are objects in this list, don't load next scene. Useful for adding functionality for fade to black.
        [ReadOnly]
        public List<Object> sceneLoadStallers = new List<Object>();



        [SerializeField]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = false, HideRemoveButton = true)]
        [InlineButton("FindAllSoScenes_EditorOnly", "Refresh")]
        public List<SceneManagerDebugContainer> allSoScenes = new List<SceneManagerDebugContainer>();

        [System.Serializable]
        public class SceneManagerDebugContainer
        {
            public SceneManagerDebugContainer(SoScene _soScene)
            {
                soScene = _soScene;
            }

            [HorizontalGroup("H"), PropertyOrder(1), HideLabel, ReadOnly]
            public SoScene soScene;

            private string SoSceneName()
            {
                if (soScene == null) return "--Null--";

                return soScene.name;// sceneByReference.SceneName;
            }

            [HorizontalGroup("H")]
            [PropertyOrder(0)]
            [Button("$SoSceneName"), DisableInEditorMode]
            public void LoadScene()
            {
                if (soScene == null) return;
                soScene.LoadScene();
            }
        }

        public override void SoOnAwake()
        {
            base.SoOnAwake();
            
            currentScene = null;
          //  currnetActiveScene = null;
            currentOpenScenes.Clear();
            currentlyOpenScenes = new List<AsyncOperationHandle>();
            instance = this;

            percentAmount.Value = 0;
            isCurrentlyLoadingScene.Value = false;
            showLoadingScreen.Value = false;
            sceneToLoadNextName = "";
            sceneToLoadNextAssetReference = null;

            GetCurrentScenes();

           
        }

        public override void SoOnStart()
        {
            base.SoOnStart();
            sceneLoadingLogicGroup?.RegisterEnabler(this, false, false);
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();
            currentScene = null;
          //  currnetActiveScene = null;
            currentOpenScenes.Clear();
            currentlyOpenScenes = new List<AsyncOperationHandle>();
            percentAmount.Value = 0;
            isCurrentlyLoadingScene.Value = false;
            showLoadingScreen.Value = false;
            sceneToLoadNextName = "";
            sceneToLoadNextAssetReference = null;

            sceneLoadingLogicGroup?.UnregisterEnabler(this);

        }

        

        public void GetCurrentScenes()
        {
#if UNITY_EDITOR
            

            currentScene = null;
          //  currnetActiveScene = null;
            currentOpenScenes.Clear();
            currentlyOpenScenes = new List<AsyncOperationHandle>();
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

            FindAllSoScenes_EditorOnly();

#else
        currentOpenScenes.Clear();
         currentlyOpenScenes = new List<AsyncOperationHandle>();

      //  currentScene = SceneManager.GetActiveScene().name;        
      //  currentOpenScenes.Add(currentScene);
#endif
        }


        private void FindAllSoScenes_EditorOnly()
        {
#if UNITY_EDITOR
            List<SoScene> foundSoScenes = RufasStatic.GetAllScriptables_ToList<SoScene>();

            allSoScenes.Clear();

            foreach (SoScene soScene in foundSoScenes)
            {
                allSoScenes.Add(new SceneManagerDebugContainer(soScene));
            }
#endif            

        }

        
        /*
        public void LoadScene(SceneReference scene)
        {
            LoadScene(scene.SceneName);
        }
        */
        public void LoadScene(AssetReference scene,string sceneName)
        {
            if (scene == null) return;
            if (currentScene == sceneName) return;
            if (currentOpenScenes.Contains(sceneName)) return;
            if (isCurrentlyLoadingScene.Value) return;
                        
            onSceneLoadTriggered.Raise();
            sceneCalledFromScene.Raise(currentScene, sceneName);
            percentAmount.Value = 0;
            isCurrentlyLoadingScene.Value = true;

            sceneLoadingLogicGroup?.EnableFromRegisteredEnabler(this);

            sceneToLoadNextAssetReference = scene;
            sceneToLoadNextName = sceneName;

            TriggerEndOfSceneBehaviours();
        }
        /*
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) { return; }
            if (currentScene == sceneName) { return; }
            if (currentOpenScenes.Contains(sceneName)) { return; }
            if (isCurrentlyLoadingScene.Value) { return; }

            onSceneLoadTriggered.Raise();
            sceneCalledFromScene.Raise(currentScene, sceneName);
            percentAmount.Value = 0;
            isCurrentlyLoadingScene.Value = true;

            sceneLoadingLogicGroup?.EnableFromRegisteredEnabler(this);

            sceneToLoadNextName = sceneName;

            TriggerEndOfSceneBehaviours();
        }
        */
        private void TriggerEndOfSceneBehaviours()
        {
            endOfScene.Raise();

            if (waitingForSceneTransitionBehaviourRoutine != null)
            {
                CoroutineMonoBehaviour.i.StopCoroutine(waitingForSceneTransitionBehaviourRoutine);
            }
            
            waitingForSceneTransitionBehaviourRoutine = WaitingForSceneTransitionBehaviour();
            CoroutineMonoBehaviour.i.StartCoroutine(waitingForSceneTransitionBehaviourRoutine);
        }

        private IEnumerator waitingForSceneTransitionBehaviourRoutine;

        private IEnumerator WaitingForSceneTransitionBehaviour()
        {
            while (true)
            {
                if(sceneLoadStallers.Count == 0)
                {
                    break;
                }

                for (int i = 0; i < sceneLoadStallers.Count; i++)
                {
                    if (sceneLoadStallers[i] == null)
                    {
                        sceneLoadStallers.RemoveAt(i);
                        i--;
                    }
                }

                yield return null;
            }

            yield return null;
            BeginTransition();
        }

       
        private void BeginTransition()
        {
         //   UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(loadingScene.SceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive, true).Completed += LoadingSceneActive;

            SceneManager.LoadSceneAsync(loadingScene.SceneName,LoadSceneMode.Additive).completed += LoadingSceneActive;
        }

        private void LoadingSceneActive(AsyncOperation _loadingScene)
        {
            showLoadingScreen.Value = true;

            CoroutineMonoBehaviour.i.CallWithDelay(CheckAndRemoveCurrentScene, 0.5f);
        }

        private void CheckAndRemoveCurrentScene()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScene.SceneName));

            /*
            //REMOVES FIRST SCENE (IF STILL ACTIVE)
            if (SceneManager.GetSceneByBuildIndex(0).IsValid() == true)
            {
                //  SceneManager.UnloadSceneAsync(0,UnloadSceneOptions.None);
            }
            */
            foreach (string next in currentOpenScenes)
            {
                //Addressables.UnloadSceneAsync()
#if UNITY_EDITOR
                //SceneManager
                SceneManager.UnloadSceneAsync(next);
#endif
            }

            foreach(AsyncOperationHandle next in currentlyOpenScenes)
            {
                //if(next)
                //if(Addressables.)
                Addressables.UnloadSceneAsync(next);
            }

            currentOpenScenes.Clear();
            currentlyOpenScenes = new List<AsyncOperationHandle>();
            LoadInNextScene();
        }
      
        public bool loadingNextSceneInProgress;
        private void LoadInNextScene()
        {
            loadingNextSceneInProgress = true;

            //    operation = SceneManager.LoadSceneAsync(sceneToLoadNextName, LoadSceneMode.Additive);
            //operation.completed += NextSceneLoadCompleted;

            operation = Addressables.LoadSceneAsync(sceneToLoadNextAssetReference, LoadSceneMode.Additive);
            operation.Completed += NextSceneLoadCompleted;

            StartLoadingStats();  
        }           

        void StartLoadingStats()
        {
            if (loadingStats != null)
            {
                CoroutineMonoBehaviour.i.StopCoroutine(loadingStats);
            }
            loadingStats = LoadingStats();
            
            CoroutineMonoBehaviour.i.StartCoroutine(loadingStats);
        }

        IEnumerator loadingStats;
        IEnumerator LoadingStats()
        {
            while (loadingNextSceneInProgress)
            {
             //   Debug.Log("Scene: " + operation.PercentComplete.ToString("F3"));
                percentAmount.Value = operation.PercentComplete;
                yield return null;
            }
        }

        private void NextSceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            loadingNextSceneInProgress = false;

            currentScene = sceneToLoadNextName;
            onSceneEntered.Raise(currentScene);
            currentOpenScenes.Add(currentScene);
            currentlyOpenScenes.Add(obj);

            SceneManager.SetActiveScene(obj.Result.Scene);

            SceneManager.UnloadSceneAsync(loadingScene.SceneName).completed += Finished;

           // LoadOutLoadingScreen();
            //Set the obj scene as the active scene?
        }

      //  private void LoadOutLoadingScreen()
     //   {
            
          //  SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
          
       // }      

        private void Finished(AsyncOperation obj)
        {
            CoroutineMonoBehaviour.i.CallWithFrameDelay(FinishedPostDelay,2);
        }

        private void FinishedPostDelay()
        {
            showLoadingScreen.Value = false;
            isCurrentlyLoadingScene.Value = false;

            sceneLoadingLogicGroup?.DisableFromRegisteredEnabler(this);
        }

        /**/
        /*
        public static SoSceneManager instance;

        [SerializeField,Required] private SceneReference loadingScene;

        [SerializeField, ReadOnly] private string currentScene;
        [SerializeField, ReadOnly] private string sceneToLoadNext;
        [SerializeField, ReadOnly] private List<string> currentOpenScenes = new List<string>();


        public BoolWithCallback isCurrentlyLoadingScene;        
        public BoolWithCallback isRequestingShowLoadingScreen;
        public FloatWithCallback sceneLoadPercentAmount;



        public event System.Action onSceneLoadTriggered;
        public event System.Action onEnterAnyScene;
        public event System.Action onExitAnyScene;


        //Whilst there are objects in this list, don't load next scene. Useful for adding functionality for fade to black.
        [ReadOnly]
        public List<Object> sceneLoadStallers = new List<Object>();


        [SerializeField]
        [SerializeReference]
        [OdinSerialize]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = false, HideRemoveButton = true)]
        [InlineButton("FindAllSoScenes","Refresh")]
        private List<SceneManagerDebugContainer> allSoScenes;


        [System.Serializable]
        public class SceneManagerDebugContainer
        {
            public SceneManagerDebugContainer(SoScene _soScene)
            {
                soScene = _soScene;
            }         

            [HorizontalGroup("H"),PropertyOrder(1), HideLabel,ReadOnly]
            public SoScene soScene;

            private string SoSceneName()
            {
                if (soScene == null) return "--Null--";

                return soScene.sceneByReference.SceneName;
            }

            [HorizontalGroup("H")]
            [PropertyOrder(0)]
            [Button("$SoSceneName"),DisableInEditorMode]
            public void LoadScene()
            {
                if (soScene == null) return;
                soScene.LoadScene();
            }
        }

        private IEnumerator sceneLoadingRoutine;

        public override void SoOnAwake()
        {
            base.SoOnAwake();
            instance = this;

            ResetValue();

            GetCurrentScenes_EditorOnly();
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();

            ResetValue();

            currentOpenScenes.Clear();
        }

        private void ResetValue()
        {

            currentScene = null;
            currentOpenScenes.Clear();
            isCurrentlyLoadingScene.Value = false;
            isRequestingShowLoadingScreen.Value = false;
            sceneLoadPercentAmount.Value = 0;
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

            FindAllSoScenes();
    #else
        currentScene = SceneManager.GetActiveScene().name;
    #endif
        }

    #if UNITY_EDITOR
        private void FindAllSoScenes()
        {
            List<SoScene> foundSoScenes = GeneralMethods.FindAllScriptableObjectsOfType<SoScene>();

            allSoScenes.Clear();

            foreach (SoScene soScene in foundSoScenes)
            {
                allSoScenes.Add(new SceneManagerDebugContainer(soScene));
            }
        }
    #endif

        public void LoadScene(SceneReference scene)
        {
            LoadScene(scene.SceneName);
        }

        public void LoadScene(string scene)
        {
            Debug.Log("SCENE: " + scene);

            //Return checks for various conditions (Read debugs)
            if (string.IsNullOrEmpty(scene)) {
                Debug.Log("Target scene to load is null, or has no name!"); return; }
            if (currentScene != null && currentScene == scene) { 
                Debug.Log("Target scene is the current scene"); return; }
            if (currentOpenScenes.Contains(scene)) { 
                Debug.Log("Target scene is already loaded (but not the current scene)"); return; }
            if (isCurrentlyLoadingScene.Value == true) { 
                Debug.Log("Already doing a scene load, so cancelling this request"); return; }

            if (onSceneLoadTriggered != null) onSceneLoadTriggered?.Invoke();
           // sceneCalledFromScene.Invoke(currentScene, scene);

            sceneLoadPercentAmount.Value = 0;
            isCurrentlyLoadingScene.Value = true;
            sceneToLoadNext = scene;
            onExitAnyScene?.Invoke();

           // asyncOperation = null;

            //Start the coroutine for transitioning scenes
            if (sceneLoadingRoutine != null) { CoroutineMonoBehaviour.i.StopCoroutine(sceneLoadingRoutine); }

            sceneLoadingRoutine = SceneLoadingRoutine();
            CoroutineMonoBehaviour.i.StartCoroutine(sceneLoadingRoutine);
        }


        //Just copy the one from colour connect
        AsyncOperation asyncOperation;
        private IEnumerator SceneLoadingRoutine()
        {
            yield return null;

            isRequestingShowLoadingScreen.Value = true;


            //Fade to black


            //Repalce this if, else with a system injecting references into a 'DelayLoadUntil' for objects setting the screen black
            //Inside, loop through the list clearing out null references just in case

            //--Delay to wait until the sceneLoadStallers is null--//
            yield return new WaitForSeconds(2);

            currentOpenScenes.Clear();
            //SceneManager.LoadScene(sceneToLoadNext);

             asyncOperation = SceneManager.LoadSceneAsync(sceneToLoadNext);


            // Allow the scene to load in the background while the game continues
            asyncOperation.allowSceneActivation = false;
            Debug.Log("HERE: 1");

            while (!asyncOperation.isDone)
            {
                // Check if the scene has finished loading
                //if (asyncOperation.progress >= 0.9f)
                //{
                    // Trigger the callback
                    //callback?.Invoke();


                    // Activate the loaded scene


                 //   break;
               // }

                yield return null;
            }

            asyncOperation.allowSceneActivation = true;

            PostSceneLoad();
        }


        private void PostSceneLoad()
        {
            Debug.Log("HERE: 2");
            CoroutineMonoBehaviour.i.StartCoroutine(PostSceneLoadRoutine());
        }

        IEnumerator PostSceneLoadRoutine()
        {
            Debug.Log("HERE: 3");
            //Delay to wait for scene load to finish
            yield return new WaitForSeconds(2);
            Debug.Log("HERE: 4");

            //Fade out of black here


            currentScene = sceneToLoadNext;
            onEnterAnyScene?.Invoke();

            currentOpenScenes.Add(currentScene);

            //Reset scene load settings to null
            isRequestingShowLoadingScreen.Value = false;
            isCurrentlyLoadingScene.Value = false;
        }
        */
    }
}
